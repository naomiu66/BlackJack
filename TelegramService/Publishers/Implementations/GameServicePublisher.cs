using MassTransit;
using TelegramService.Publishers.Abstractions;

namespace TelegramService.Publishers.Implementations
{
    public class GameServicePublisher : Publisher, IGameServicePublisher
    {
        public GameServicePublisher(IBus bus) : base(bus)
        {
        }
    }
}
