using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

public class Gpt4AllClient
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static string Gpt4allApiUrl;
    private static string Model;
    private static string SystemMessage;

    // Store chat history per user (userId -> list of messages)
    private static Dictionary<ulong, List<dynamic>> chatMemory = new();

    public static void Initialize(string localHost, string model, string systemMessage)
    {
        Gpt4allApiUrl = $"http://localhost:{localHost}/v1/chat/completions";
        Model = model;
        SystemMessage = systemMessage;
    }

    public static async Task<string> GetResponseAsync(ulong userId, string username, string userPrompt)
    {
        try
        {
            // Initialize chat memory if user is not in the dictionary
            if (!chatMemory.ContainsKey(userId))
            {
                chatMemory[userId] = new List<dynamic>
                {
                    new { role = "system", content = SystemMessage }
                };
            }

            // Add user message to memory
            chatMemory[userId].Add(new { role = "user", content = $"{username}: {userPrompt}" });

            // Keep only the last 10 messages (excluding system message) to prevent overflow
            if (chatMemory[userId].Count > 10)
            {
                chatMemory[userId].RemoveAt(1);
            }

            var requestBody = new
            {
                model = Model,
                messages = chatMemory[userId],
                temperature = 0.7,
                max_tokens = 1500 // Max tokens for maybe around 1000 words
            };

            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(Gpt4allApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"ERROR: Failed to get response. Status code: {response.StatusCode}");
                return "**Error getting AI response.**";
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"\nRAW AI RESPONSE: {responseBody}");  // Debugging the AI response

            var jsonResponse = JsonConvert.DeserializeObject<Gpt4allResponse>(responseBody);

            // Extract AI's message
            string aiResponse = jsonResponse.Choices[0].Message.Content;

            if (string.IsNullOrWhiteSpace(aiResponse))
            {
                Console.WriteLine("AI response content is empty.");
                return "**Error getting AI response.**";
            }

            // Store AI response in memory
            chatMemory[userId].Add(new { role = "assistant", content = aiResponse });

            // Remove <think>...</think> from DeepSeek model responses
            string cleanedResponse = Regex.Replace(aiResponse, @"<think>.*?</think>", "", RegexOptions.Singleline);

            return cleanedResponse.Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine("================= Error =================");
            Console.WriteLine(ex.Message);
            Console.WriteLine("================= Error =================");
            return "**Oops! Something went wrong. Please try again later.**";
        }
    }

    public static void ClearUserChatHistory(ulong userId)
    {
        chatMemory.Remove(userId);
    }

    public static void ResetBot()
    {
        chatMemory.Clear();
    }

    private class Gpt4allResponse
    {
        [JsonProperty("choices")]
        public List<Choice> Choices { get; set; }
    }

    private class Choice
    {
        [JsonProperty("message")]
        public Message Message { get; set; }
    }

    private class Message
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
