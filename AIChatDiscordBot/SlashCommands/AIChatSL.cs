using AIChatDiscordBot.Config;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
namespace AIChatDiscordBot.SlashCommands
{
    public class AIChatSL : ApplicationCommandModule
    {
        private static string _aiProvider;
        private static string _modelName;

        static AIChatSL()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            if (File.Exists("configGPT4All.json"))
            {
                _aiProvider = "GPT4All";
            }
            else if (File.Exists("configOllama.json"))
            {
                _aiProvider = "Ollama";
            }
            else
            {
                _aiProvider = "None";
            }
            // Load model name from JSON
            _modelName = JSONReader.GetModelName();
        }

        [SlashCommand("ask", "Ask the AI a question")]
        public async Task AskAI(InteractionContext ctx, [Option("message", "Enter your message to the AI")] string message)
        {
            await ctx.DeferAsync();
            ulong userId = ctx.User.Id;
            string username = ctx.User.Username;
            string aiResponse = "AI Provider Not Found.";

            if (_aiProvider == "GPT4All")
            {
                aiResponse = await Gpt4AllClient.GetResponseAsync(userId, username, message);
            }
            else if (_aiProvider == "Ollama")
            {
                aiResponse = await OllamaClient.GetResponseAsync(userId, username, message);
            }

            var aiResponseEmbed = new DiscordEmbedBuilder()
            {
                Color = aiResponse.Contains("Oops! Something went wrong") ? DiscordColor.Red : new DiscordColor(52, 152, 219),
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = $"{username} asked:\n{message}",
                    IconUrl = ctx.User.AvatarUrl
                },
                Title = $"Model: {_modelName}\n\nAI response",
                Description = aiResponse,
            };
            Console.WriteLine($"\n{DateTime.Now}");
            Console.WriteLine($"=============\n {username} asked:\n{message}\n=============\n");
            Console.WriteLine($"-----------------\n AI {_modelName} responded:\n{aiResponse}\n-----------------\n");

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(aiResponseEmbed));
        }

        [SlashCommand("forgetme", "Start a fresh conversation with the AI only for you.")]
        public async Task ForgetMe(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            ulong userId = ctx.User.Id;

            // Reset chat history (for specific user that used the command)
            OllamaClient.ClearUserChatHistory(userId);

            var newSessionEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.HotPink,
                Title = $"New AI chat session for {ctx.User.Username}",
                Description = "The session was deleted. You can now start a whole new conversation.",
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(newSessionEmbed));
        }

        [SlashCommand("reset", "Resets all the users chats and starts a whole new conversation for everyone.")]
        public async Task NewChat(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            ulong userId = ctx.User.Id;

            // Reset chat history for everyone
            OllamaClient.ResetBot();

            var newSessionEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Gold,
                Title = "New AI chat session for everyone",
                Description = "All sessions are deleted. You can now start a whole new conversation. The bot doest remember anyone from past talk",
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(newSessionEmbed));
        }

        [SlashCommand("help", "Show all the commands for the AI chat bot.")]
        public async Task Help(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var helpEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Green,
                Title = "AI chat bot commands",
                Description = "**/ask** - To tell something to the bot \n" +
                              "**/forgetme** - Forgets only the conversations that had with you. \n" +
                              "**/reset** - Resets the bot. Everyone starts with a new conversation.\n" +
                              "**/help** - Show all the commands for the bot"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(helpEmbed));
        }
    }
}
