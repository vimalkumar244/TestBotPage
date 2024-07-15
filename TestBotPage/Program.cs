using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EchoBot
{
    class Program
    {
        private static readonly string BotToken = "7349408569:AAGoPhWiQQ-sb2L24Tqoi8Z0gB93Wofg9Ws";
        private static TelegramBotClient BotClient;

        static async Task Main(string[] args)
        {
            BotClient = new TelegramBotClient(BotToken);

            var me = await BotClient.GetMeAsync();
            Console.WriteLine($"Hello! My name is {me.FirstName}");

            using var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            BotClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Console.WriteLine("Bot is running...");
            Console.ReadLine();
            cts.Cancel(); // Stop bot gracefully
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                var messageText = update.Message.Text.ToLower();
                var chatId = update.Message.Chat.Id;

                switch (messageText)
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(chatId, "Hello! I am your bot. How can I assist you today?", cancellationToken: cancellationToken);
                        break;

                    case "/help":
                        await botClient.SendTextMessageAsync(chatId, "I am here to help! Send me any message and I will echo it back.", cancellationToken: cancellationToken);
                        break;

                    case "/kowshik":
                        await botClient.SendTextMessageAsync(chatId, "Nee oru gay da.", cancellationToken: cancellationToken);
                        break;

                    default:
                        await botClient.SendTextMessageAsync(chatId, $"You said: {update.Message.Text}", cancellationToken: cancellationToken);
                        break;
                }
            }
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"An error occurred: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
