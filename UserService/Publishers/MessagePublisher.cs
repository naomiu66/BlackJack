using MassTransit;

namespace UserService.Publishers
{
    public class MessagePublisher : IMessagePublisher 
    {
        private readonly IBus _bus;

        public MessagePublisher(IBus bus)
        {
            _bus = bus;
        }
    }
}
