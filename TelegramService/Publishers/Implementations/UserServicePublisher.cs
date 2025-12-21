using MassTransit;
using TelegramService.Publishers.Abstractions;

namespace TelegramService.Publishers.Implementations
{
    public class UserServicePublisher : Publisher, IUserServicePublisher
    {
        public UserServicePublisher(IBus bus) : base(bus)
        {
        }
    }
}
