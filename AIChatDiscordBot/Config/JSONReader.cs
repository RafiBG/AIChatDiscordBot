using Newtonsoft.Json;

namespace AIChatDiscordBot.Config
{
    internal class JSONReader
    {
        public string token { get; set; }
        public string model { get; set; }
        public string localHost { get; set; }
        public string systemMessage { get; set; }

        private string filePath;

        private string yourDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public static string gpt4AllConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configGPT4All.json");
        private static string defaultConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configOllama.json");

        public async Task ReadJSON()
        {
            if (File.Exists(gpt4AllConfig))
            {
                this.filePath = gpt4AllConfig;
                //return data.model;
            }
            else if (File.Exists(defaultConfig))
            {
                this.filePath = defaultConfig;
            }
            else
            {
                Console.WriteLine("The file configOllama.json or configGPT4All.json is missing.\n");
                Console.WriteLine(@"First go to AIChatDiscordBot\Config and open configOllama.json or configGPT4All.json");
                Console.WriteLine($"\nCopy the configOllama.json or configGPT4All.json after you added your info and paste it in {yourDirectory}\n" +
                    $"\nFinal step restart the program. The other message will show again if the token is not corectly put.");
                return;
            }

            using (StreamReader sr = new StreamReader(this.filePath))
            {
                string json = await sr.ReadToEndAsync();
                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);

                this.token = data.token;
                this.model = data.model;
                this.localHost = data.localHost;
                this.systemMessage = data.systemMessage;

                // For debug
                Console.WriteLine($"Loaded Config:");
                Console.WriteLine($"Model: {this.model}");
                Console.WriteLine($"LocalHost: {this.localHost}");
                //Console.WriteLine($"System Message: {this.systemMessage}");

            }
            Console.WriteLine($"Loaded configuration from\n {this.filePath}");
        }

        public static string GetModelName()
        {
            //string gpt4AllConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configGPT4All.json");
            //string defaultConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configOllama.json");

            if (File.Exists(gpt4AllConfig))
            {
                string json = File.ReadAllText(gpt4AllConfig);
                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);
                return data.model;
            }
            else if (File.Exists(defaultConfig))
            {
                string json = File.ReadAllText(defaultConfig);
                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);
                return data.model;
            }

            return "Unknown model";
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