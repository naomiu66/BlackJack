namespace TelegramService.Payloads
{
    public record UserPayload
    {
        public long UserId { get; init; }
        public long ChatId { get; init; }
        
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
    }
}
