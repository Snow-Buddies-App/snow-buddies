using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowBuddies.Application.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
    }
}
