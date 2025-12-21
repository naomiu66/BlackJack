using TelegramService.Handlers.Commands.Abstractions;

namespace TelegramService.Handlers.Commands.Router
{
    public interface ICommandRouter
    {
        ICommand? Route(string input, out string[] args);
    }
}
