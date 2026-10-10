using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities.Enums;

namespace SnowBuddies.Application.Dtos
{
    public class UserProfileDto
    {
        public Guid UserProfileId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly DateOfBirth { get; set; } 
        
    }
}
