using Telegram.Bot.Types;
using TelegramService.Handlers.Abstractions;
using TelegramService.Services.Abstractions;

namespace TelegramService.Services.Implementations
{
    public class UpdateService : IUpdateService
    {
        private readonly IMessageHandler _messageHandler;
        private readonly ICallbackHandler _callbackHandler;

        public UpdateService(ICallbackHandler callbackHandler, IMessageHandler messageHandler)
        {
            _callbackHandler = callbackHandler;
            _messageHandler = messageHandler;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    if (update.Message != null)
                    {
                        Console.WriteLine($"Received message: {update.Message.Text}");
                        await _messageHandler.HandleAsync(update.Message);
                    }
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                    if (update.CallbackQuery != null) 
                    {
                        Console.WriteLine($"Received callback query: {update.CallbackQuery.Data}");
                        await _callbackHandler.HandleAsync(update.CallbackQuery);
                    }
                    break;
                default:
                    Console.WriteLine($"Unhandled update type: {update.Type}");
                    break;
            }
        }
    }
}
