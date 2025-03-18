# Description
Made with C# on .NET 9.0 and libraries: DSharpPlus, DsharpPlus.Interactivity, DSharpPlus.SlashCommands, Newtonsoft.Json
# How to use it 
Go to Discord developer portal link: https://discord.com/developers/applications. Make your own discord bot and get the token that will be used in config.json. Before you leave the website go to Bot and turn on the ones that you see on the image. ![privilage](https://github.com/user-attachments/assets/f6a7ae67-acf6-4d11-a479-7b55df3fab02)

Now the Bot Permissions are shown in the image that you must check to work. ![bot](https://github.com/user-attachments/assets/61b00634-6aee-474f-99e3-549f142380e4)

Install Ollama from this link: https://ollama.com/
From the official ollama website you can download AI model for your discord bot: https://ollama.com/search
Open your console and enter: ollama serve
It will show you the error and the last 5 numbers that you need to put in config.json to connect the AI.
![comm](https://github.com/user-attachments/assets/c8af5b48-042d-4a74-a9d5-e5a5b798a010)

Ollama must be running in the background to be able to connect to your local AI chat bot for discord.
Run the command in the console: ollama list
Copy the full name of the model you downloaded. We will need it for config.json
Example of what you will be looking for the name of the model in the console. ![ModelName](https://github.com/user-attachments/assets/cb687521-ea53-44fe-8a0c-28db29f85d5e)


For first time running you must open AIChatDiscordBot.sln . You will need Visual Studio to open it. Download link for Visual Studio : https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&passive=false&cid=2030

When you open it in Visual Studio, press AIChatDiscordBot up there. Here is an image of what you will be looking at.
![start vis](https://github.com/user-attachments/assets/65832a87-c246-40c1-a82d-adc91400351a)
You can now close the console and Visual Studio.

In the folder Config there is config.json add your Discord token, the 5 numbers of your server, the exact name of the model that you downloaded, then copy the whole file config.json and go to AIChatDiscordBot\AIChatDiscordBot\bin\Debug\net9.0 and paste it there, also there is your AIChatDiscordBot.exe to start the program.

If you get in the console this error: "No connection could be made because the target machine actively refused it." Check if your "localhost" number is correct and token for config.json server or create one.

Slash commands
/ask (your message)
/forgetme - Start a fresh conversation with the AI only for you.
/reset - Resets all the user's chats and starts a whole new conversation for everyone.
/help - Show all the commands for the AI chat bot.

# Results 
![ask0](https://github.com/user-attachments/assets/30b107ec-5cfc-41f0-b8e8-1ac7398f250e)

![ask1](https://github.com/user-attachments/assets/6487ee03-2079-4c34-9d85-d7bc83233fd1)
