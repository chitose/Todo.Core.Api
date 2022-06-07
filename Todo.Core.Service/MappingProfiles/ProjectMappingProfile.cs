using AutoMapper;
using Todo.Core.Domain.Project;
using Todo.Core.Common.Extensions;
namespace Todo.Core.Service.MappingProfiles;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectUpdateInfo, Persistence.Entities.Project>().IgnoreNullProperties();

    }
}