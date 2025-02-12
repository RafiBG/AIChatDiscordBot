# Description
Made with C# on .NET 9.0 and libraries: DSharpPlus, DsharpPlus.Interactivity, DSharpPlus.SlashCommands, Newtonsoft.Json
# How to use it
Go to Discord developer portal link: https://discord.com/developers/applications. Make your own discord bot and get the token that will be used in config.json.
Before you leave the website go to Bot and turn on the ones that you see on the image.
![PrivilegedGeteway](https://github.com/user-attachments/assets/67bb9842-b892-4327-b2db-98c7c4801db7)
Now the Bot Permissions are shown in the image that you must check to work.
![AIBotPersmissions](https://github.com/user-attachments/assets/84518b75-1c69-4d82-a428-230b1444044f)
<br>
Install Ollama from this link: https://ollama.com/ <br>
Open your console and enter: ollama serve <br>
It will show you error and the last 5 numbers that you need to put in config.json to connect the AI. <br>
Ollama must run in the background to be able to connect to your local AI chat bot for discord.

In folder Config there is config.json add your Discord token then copy the whole file config.json and go to
AIChatDiscordBot\AIChatDiscordBot\bin\Debug\net9.0 and paste it there also there is your AIChatDiscordBot.exe
to start the program. <br /> <br />
If you get in the console this error: "No connection could be made because the target machine actively refused it." check if your "localHost" number is correct and token for config.json server or create one. <br />

# Slash commands
/ask (your message) <br />
/forgetme - Start a fresh conversation with the AI only for you. <br />
/reset - Resets all the users chats and starts a whole new conversation for everyone. <br />
/help - Show all the commands for the AI chat bot.
