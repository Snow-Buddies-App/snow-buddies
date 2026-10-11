namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IPasswordService
    {
       void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
       bool VerifyPasswordHash(string password, byte[]? storedPasswordHash, byte[]? storedPasswordSalt);
    }
}
