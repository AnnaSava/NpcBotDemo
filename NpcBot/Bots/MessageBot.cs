using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace NpcBot.Bots
{
    public class MessageBot
    {
        readonly int SLEEP;
        readonly ITelegramBotClient botClient;

        public MessageBot(BotModel bot, int sleep)
        {
            SLEEP = sleep;
            botClient = new TelegramBotClient(bot.Token);
        }

        public void Start()
        {
            Console.WriteLine("I'm bot " + botClient.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        public async Task Send(long chatId, string message)
        {
            await botClient.SendTextMessageAsync(new ChatId(chatId), message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            Thread.Sleep(SLEEP);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message == null || message?.Chat == null) return;
                if (message?.Text?.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Hello world!");
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "I'm working well");
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
