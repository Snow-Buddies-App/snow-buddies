namespace SnowBuddies.Application.Dtos
{
    public class GoogleUserInfo
    {
        public string Email { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string? Picture { get; set; }
    }
}
