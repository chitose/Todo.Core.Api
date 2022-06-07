using AutoMapper;
using Todo.Core.Common.Extensions;
using Todo.Core.Domain.Dto;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Api.MappingProfiles;

public class UserDtoMappingProfile : Profile
{
    public UserDtoMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, User>().IgnoreNullProperties();
    }
}