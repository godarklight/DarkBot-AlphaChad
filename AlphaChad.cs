using System;
using System.Threading.Tasks;
using DarkBot;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DarkBot_AlphaChad
{
    public class AlphaChad : BotModule
    {
        private DiscordSocketClient _client = null;
        private string findMessage = "I know this is annoying";
        private string replyMessage = "Cucked!";

        public Task Initialize(IServiceProvider service)
        {
            _client = service.GetService(typeof(DiscordSocketClient)) as DiscordSocketClient;
            _client.Ready += OnReady;
            _client.MessageReceived += HandleMessage;
            _client.SlashCommandExecuted += HandleCommand;
            return Task.CompletedTask;
        }

        private async Task OnReady()
        {
            await SetupCommands();
            Log(LogSeverity.Info, "AlphaChad ready!");
        }

        private async Task SetupCommands()
        {
            SlashCommandBuilder scb = new SlashCommandBuilder();
            scb.WithName("ping");
            scb.WithDescription("Check if the bot is alive");
            try
            {
                Log(LogSeverity.Error, $"Ping command set up");
                await _client.CreateGlobalApplicationCommandAsync(scb.Build());
            }
            catch (Exception e)
            {
                Log(LogSeverity.Error, $"Error setting up slash command: {e.Message}");
            }

        }

        private async Task HandleCommand(SocketSlashCommand command)
        {
            await command.RespondAsync($"Yo!");
        }

        private async Task HandleMessage(SocketMessage socketMessage)
        {
            SocketUserMessage message = socketMessage as SocketUserMessage;
            if (message == null)
            {
                return;
            }
            SocketTextChannel channel = message.Channel as SocketTextChannel;
            if (channel == null)
            {
                return;
            }
            await CheckMessage(message, channel);
            if (message.Author.Id == _client.CurrentUser.Id)
            {
                if (message.Content == replyMessage)
                {
                    Log(LogSeverity.Info, "Removing own message");
                    await DeleteMessage(message, channel);
                }
            }
        }

        private async Task CheckMessage(IMessage message, SocketTextChannel channel)
        {
            if (message.Author.Id != 418412306981191680)
            {
                return;
            }
            if (message.Content != null && message.Content.Contains("I'm vewy sowwy about cwashing"))
            {
                Log(LogSeverity.Info, "Cucking BMO - Crashed");
                await channel.SendMessageAsync("You're pathetic BMO. I'm coming over to fuck your wife now.");
            }
            if (message.Embeds != null)
            {
                bool deleteThisMessage = false;
                foreach (IEmbed e in message.Embeds)
                {
                    if (e != null && e.Description != null && e.Description.Contains(findMessage))
                    {
                        deleteThisMessage = true;
                    }
                }
                if (deleteThisMessage)
                {
                    Log(LogSeverity.Info, "Cucking BMO");
                    await ReplyMessage(message, channel);
                }
            }
        }

        private void Log(LogSeverity severity, string text)
        {
            LogMessage logMessage = new LogMessage(severity, "AlphaChad", text);
            Program.LogAsync(logMessage);
        }

        private async Task ReplyMessage(IMessage message, SocketTextChannel channel)
        {
            await message.DeleteAsync();
            await channel.SendMessageAsync(replyMessage);
        }

        private async Task DeleteMessage(IMessage message, SocketTextChannel channel)
        {
            await Task.Delay(5000);
            await message.DeleteAsync();
        }
    }
}
