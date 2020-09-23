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
            _client.MessageReceived += HandleMessage;
            return Task.CompletedTask;
        }

        private Task HandleMessage(SocketMessage message)
        {
            SocketUserMessage sum = message as SocketUserMessage;
            if (sum == null)
            {
                return Task.CompletedTask;
            }
            SocketTextChannel stc = sum.Channel as SocketTextChannel;
            if (stc == null)
            {
                return Task.CompletedTask;
            }
            if (sum.Author.Id == 418412306981191680 && sum.Embeds != null)
            {
                bool deleteThisMessage = false;
                foreach (IEmbed e in sum.Embeds)
                {
                    if (e != null && e.Description.Contains(findMessage))
                    {
                        deleteThisMessage = true;
                    }
                }
                if (deleteThisMessage)
                {
                    Log(LogSeverity.Info, "Cucking BMO");
                    ReplyMessage(sum, stc);
                }
            }
            if (sum.Author.Id == _client.CurrentUser.Id)
            {
                if (sum.Content == replyMessage)
                {
                    Log(LogSeverity.Info, "Removing own message");
                    DeleteMessage(sum, stc);
                }
            }
            return Task.CompletedTask;
        }

        private void Log(LogSeverity severity, string text)
        {
            LogMessage logMessage = new LogMessage(severity, "AlphaChad", text);
            Program.LogAsync(logMessage);
        }

        private async void ReplyMessage(SocketUserMessage sum, SocketTextChannel stc)
        {
            await sum.DeleteAsync();
            await stc.SendMessageAsync(replyMessage);
        }

        private async void DeleteMessage(SocketUserMessage sum, SocketTextChannel stc)
        {
            await Task.Delay(5000);
            await sum.DeleteAsync();
        }
    }
}
