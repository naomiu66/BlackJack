using Telegram.Bot.Types;

namespace TelegramService.Handlers.Abstractions
{
    public interface ICallbackHandler
    {
        Task HandleAsync(CallbackQuery callback);
    }
}
