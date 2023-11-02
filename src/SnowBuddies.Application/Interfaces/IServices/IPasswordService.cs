using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IPasswordService
    {
       void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
       bool VerifyPassword(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
