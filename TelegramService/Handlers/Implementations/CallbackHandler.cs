using Telegram.Bot.Types;
using TelegramService.Handlers.Abstractions;

namespace TelegramService.Handlers.Implementations
{
    public class CallbackHandler : ICallbackHandler
    {
        public Task HandleAsync(CallbackQuery callback)
        {
            throw new NotImplementedException();
        }
    }
}
