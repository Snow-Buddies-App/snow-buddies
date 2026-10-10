using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SnowBuddies.Api.Models
{
    public class UserProfileModel
    {
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }

        [MaxLength(30)]
        public string? FirstName { get; set; }

        [MaxLength(30)]
        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }
    }
}