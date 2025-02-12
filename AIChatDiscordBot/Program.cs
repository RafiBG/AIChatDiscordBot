using AIChatDiscordBot.Config;
using AIChatDiscordBot.SlashCommands;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace AIChatDiscordBot;
public class Program
{
    public static DiscordClient Client { get; set; }
    static async Task Main(string[] args)
    {
        var jsonReader = new JSONReader();
        await jsonReader.ReadJSON();

        if (string.IsNullOrWhiteSpace(jsonReader.token))
        {
            Console.WriteLine("The token in config.json is missing or invalid. Please provide a valid bot token.");
        }
        var discordConfig = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = jsonReader.token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
        };

        Client = new DiscordClient(discordConfig);
        Client.Ready += Client_Ready;

        var slashCommandConfig = Client.UseSlashCommands();

        // This may take up to 1 hour for discord to see the new changes and make it work
        slashCommandConfig.RegisterCommands<AIChatSL>();

        OllamaClient.Initialize(jsonReader.localHost, jsonReader.model,jsonReader.systemMessage);

        // Connect for the bot to be online
        await Client.ConnectAsync(new DiscordActivity(" /help",ActivityType.ListeningTo));
        await Task.Delay(-1);
    }
    private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
    {
        Console.WriteLine("Bot is ready");
        return Task.CompletedTask;
    }
}