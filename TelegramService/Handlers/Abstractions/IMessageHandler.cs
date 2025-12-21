using Telegram.Bot.Types;

namespace TelegramService.Handlers.Abstractions
{
    public interface IMessageHandler
    {
        Task HandleAsync(Message message);
    }
}
