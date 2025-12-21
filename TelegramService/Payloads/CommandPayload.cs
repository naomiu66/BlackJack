namespace TelegramService.Payloads
{
    public record CommandPayload
    {
        public UserPayload User { get; init; } = new UserPayload();
        public required string[] Args { get; init; }
        public int? MessageId { get; init; }
    }
}
