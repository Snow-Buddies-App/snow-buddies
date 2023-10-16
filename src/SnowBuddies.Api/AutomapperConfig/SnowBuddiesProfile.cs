using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Api.Models;

namespace SnowBuddies.Api.AutomapperConfig
{
    public class SnowBuddiesProfile : AutoMapper.Profile
    {
        public SnowBuddiesProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
