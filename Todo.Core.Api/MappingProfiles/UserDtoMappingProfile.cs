using AutoMapper;
using Todo.Core.Domain.Dto;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Api.MappingProfiles;

public class UserDtoMappingProfile : Profile
{
    public UserDtoMappingProfile()
    {
        CreateMap<User, UserDto>();

    }
}