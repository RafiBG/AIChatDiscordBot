using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

public class OllamaClient
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static string OllamaApiUrl;
    private static string Model;
    private static string SystemMessage;

    // Store chat history per user (userId -> list of messages)
    private static Dictionary<ulong, List<dynamic>> chatMemory = new();

    public static void Initialize(string localHost, string model, string systemMessage)
    {
        OllamaApiUrl = $"http://localhost:{localHost}/api/chat";
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
                stream = false
            };

            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(OllamaApiUrl, content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            var jsonResponse = JsonConvert.DeserializeObject<OllamaResponse>(result);

            if (jsonResponse?.Message?.Content == null)
                return "Error getting AI response.";

            // Store AI response in memory
            string aiResponse = jsonResponse.Message.Content;
            chatMemory[userId].Add(new { role = SystemMessage, content = aiResponse });

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

    private class OllamaResponse
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
