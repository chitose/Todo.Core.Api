using AutoMapper;
using Todo.Core.Domain.Project;

namespace Todo.Core.Service.MappingProfiles;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectUpdateInfo, Persistence.Entities.Project>()
            .ForAllMembers(x =>
                x.Condition((src, dest, val) => val != null));
    }
}