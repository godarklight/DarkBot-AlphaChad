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
        private string[] pings = { "hi", "hello", "hey", "ping" };

        public Task Initialize(IServiceProvider service)
        {
            _client = service.GetService(typeof(DiscordSocketClient)) as DiscordSocketClient;
            _client.Ready += OnReady;
            _client.MessageReceived += HandleMessage;
            return Task.CompletedTask;
        }

        private Task OnReady()
        {
            Log(LogSeverity.Info, "AlphaChad ready!");
            return Task.CompletedTask;
        }

        private Task HandleMessage(SocketMessage socketMessage)
        {
            SocketUserMessage message = socketMessage as SocketUserMessage;
            if (message == null)
            {
                return Task.CompletedTask;
            }
            SocketTextChannel channel = message.Channel as SocketTextChannel;
            if (channel == null)
            {
                return Task.CompletedTask;
            }
            CheckMessage(message, channel);
            CheckPing(message, channel);
            if (message.Author.Id == _client.CurrentUser.Id)
            {
                if (message.Content == replyMessage)
                {
                    Log(LogSeverity.Info, "Removing own message");
                    DeleteMessage(message, channel);
                }
            }
            return Task.CompletedTask;
        }

        private void CheckMessage(IMessage message, SocketTextChannel channel)
        {
            if (message.Author.Id == 418412306981191680 && message.Embeds != null)
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
                    ReplyMessage(message, channel);
                }
            }
        }

        private void CheckPing(IMessage message, SocketTextChannel channel)
        {
            bool mentionsUs = false;
            foreach (ulong user in message.MentionedUserIds)
            {
                if (user == _client.CurrentUser.Id)
                {
                    mentionsUs = true;
                }
            }
            if (!mentionsUs)
            {
                return;
            }
            bool isPing = false;
            if (message.Content != null)
            {
                string lowerText = message.Content.ToLower();
                foreach (string testString in pings)
                {
                    if (lowerText.Contains(testString))
                    {
                        isPing = true;
                    }
                }

            }
            if (isPing)
            {
                channel.SendMessageAsync("Yo!");
            }
        }

        private void Log(LogSeverity severity, string text)
        {
            LogMessage logMessage = new LogMessage(severity, "AlphaChad", text);
            Program.LogAsync(logMessage);
        }

        private async void ReplyMessage(IMessage message, SocketTextChannel channel)
        {
            await message.DeleteAsync();
            await channel.SendMessageAsync(replyMessage);
        }

        private async void DeleteMessage(IMessage message, SocketTextChannel channel)
        {
            await Task.Delay(5000);
            await message.DeleteAsync();
        }
    }
}
