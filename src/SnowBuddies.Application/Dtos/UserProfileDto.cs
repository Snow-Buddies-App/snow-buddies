namespace SnowBuddies.Application.Dtos
{
    public class UserProfileDto
    {
        public Guid UserProfileId { get; set; }
        public Guid UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
