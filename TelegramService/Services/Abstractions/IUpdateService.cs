using Telegram.Bot.Types;

namespace TelegramService.Services.Abstractions
{
    public interface IUpdateService
    {
        Task HandleUpdateAsync(Update update);
    }
}
