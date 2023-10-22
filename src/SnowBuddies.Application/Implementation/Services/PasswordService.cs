﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SnowBuddies.Application.Interfaces.IServices;

namespace SnowBuddies.Application.Implementation.Services
{
    public class PasswordService : IPasswordService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            ValidateNonEmptyPassword(password);
            using(var hmac = new HMACSHA512()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            throw new NotImplementedException();
        }

        private string ValidateNonEmptyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException();
            }
            return password;
        }
    }
}
