namespace TelegramService.Handlers.Commands.Abstractions
{
    public interface ICommand
    {
        string Name { get; }
        // TODO: Consider using CommandPayload record
        Task ExecuteAsync(string[] args);
    }
}
