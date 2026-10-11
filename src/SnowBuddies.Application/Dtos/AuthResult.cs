namespace SnowBuddies.Application.Dtos
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public Guid UserId { get; set; }
    }
}
