using Newtonsoft.Json;

namespace AIChatDiscordBot.Config
{
    internal class JSONReader
    {
        public string token { get; set; }
        public string model { get; set; }
        public string localHost { get; set; }
        public string systemMessage { get; set; }

        public async Task ReadJSON()
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

        internal sealed class JSONStructure
        {
            public string token { get; set; }
            public string model { get; set; }
            public string localHost { get; set; }
            public string systemMessage { get; set; }
        }
    }
}