
using System.Reflection.Metadata.Ecma335;
using TelegramService.Handlers.Commands.Abstractions;

namespace TelegramService.Handlers.Commands.Router
{
    public class CommandRouter : ICommandRouter
    {
        private readonly IEnumerable<ICommand> _commands;

        public CommandRouter(IEnumerable<ICommand> commands)
        {
            _commands = commands;
        }

        public ICommand? Route(string input, out string[] args)
        {
            args = Array.Empty<string>();

            if (string.IsNullOrEmpty(input)) return null;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) return null;

            var name = parts[0];
            args = parts.Skip(1).ToArray();

            return _commands.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
