using Telegram.Bot.Types;
using TelegramService.Handlers.Abstractions;
using TelegramService.Handlers.Commands.Router;

namespace TelegramService.Handlers.Implementations
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ICommandRouter _router;

        public MessageHandler(ICommandRouter router)
        {
            _router = router;
        }

        public async Task HandleAsync(Message message)
        {
            var messageText = message.Text;

            if (!string.IsNullOrEmpty(messageText) && messageText.StartsWith("/")) 
            {
                var command = _router.Route(messageText, out var args);
                // TODO: use commandPayload here
                if(command != null) await command.ExecuteAsync(args);
            }
        }
    }
}
