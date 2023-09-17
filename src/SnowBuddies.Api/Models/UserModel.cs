using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities.Enums;

namespace SnowBuddies.Api.Models
{
    public class UserModel
    {

    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AccountStatus AccountStatus { get; set; }
    
    }
}