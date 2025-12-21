using MassTransit;

namespace TelegramService.Publishers.Abstractions
{
    public abstract class Publisher
    {
        protected readonly IBus _bus;

        public Publisher(IBus bus)
        {
            _bus = bus;
        }
    }
}
