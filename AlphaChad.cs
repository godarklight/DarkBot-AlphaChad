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
        private string replyMessage = "Cucked!";

        public async Task Initialize(IServiceProvider service)
        {
            _client = service.GetService(typeof(DiscordSocketClient)) as DiscordSocketClient;
            _client.MessageReceived += HandleMessage;
        }

        public async Task HandleMessage(SocketMessage message)
        {
            SocketUserMessage sum = message as SocketUserMessage;
            if (sum == null)
            {
                return;
            }
            SocketTextChannel stc = sum.Channel as SocketTextChannel;
            if (stc == null)
            {
                return;
            }
            if (sum.Author.Id == 418412306981191680)
            {
                if (sum.Content.Contains("Have I done well today?"))
                {
                    ReplyMessage(sum, stc);
                }
            }
            if (sum.Author.Id == _client.CurrentUser.Id)
            {
                if (sum.Content == replyMessage)
                {
                    DeleteMessage(sum, stc);
                }
            }

        }

        public async Task ReplyMessage(SocketUserMessage sum, SocketTextChannel stc)
        {
            sum.DeleteAsync();
            Task<RestUserMessage> rum = stc.SendMessageAsync(replyMessage);
        }

        public async Task DeleteMessage(SocketUserMessage sum, SocketTextChannel stc)
        {
            await Task.Delay(5000);
            sum.DeleteAsync();
        }
    }
}
