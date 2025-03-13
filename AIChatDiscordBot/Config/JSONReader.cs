using Newtonsoft.Json;

namespace AIChatDiscordBot.Config
{
    internal class JSONReader
    {
        public string token { get; set; }
        public string model { get; set; }
        public string localHost { get; set; }
        public string systemMessage { get; set; }

        private string yourDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public async Task ReadJSON()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(directory,"config.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("The file config.json is missing.\n");
                Console.WriteLine(@"First go to AIChatDiscordBot\Config and open config.json");
                Console.WriteLine($"\nCopy the config.json after you added your info and paste it in {yourDirectory}\n" +
                    $"\nFinal step restart the program. The other message will show again if the token is not corectly put.");
            }
            else 
            {
                using (StreamReader sr = new StreamReader("config.json"))
                {
                    string json = await sr.ReadToEndAsync();
                    JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);

                    this.token = data.token;
                    this.model = data.model;
                    this.localHost = data.localHost;
                    this.systemMessage = data.systemMessage;
                }
            }
        }

        internal sealed class JSONStructure
        {
            public string token { get; set; }
            public string model { get; set; }
            public string localHost { get; set; }
            public string systemMessage { get; set; }
        }
    }
}