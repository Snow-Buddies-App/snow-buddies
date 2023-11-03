﻿using AutoMapper;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.AutoMapperConfiguration
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
