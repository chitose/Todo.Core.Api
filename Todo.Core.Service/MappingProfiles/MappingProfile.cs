using AutoMapper;
using Todo.Core.Domain.Project;
using Todo.Core.Common.Extensions;
using Todo.Core.Persistence.Extensions;

namespace Todo.Core.Service.MappingProfiles;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectUpdateInfo, Persistence.Entities.Project>().IgnoreNullProperties();

        CreateMap<Persistence.Entities.Project, Persistence.Entities.Project>().IgnoreBaseProps();

        CreateMap<Persistence.Entities.ProjectSection, Persistence.Entities.ProjectSection>().IgnoreBaseProps();

        CreateMap<Persistence.Entities.TodoTask, Persistence.Entities.TodoTask>().IgnoreBaseProps();
        
        CreateMap<Persistence.Entities.ProjectComment, Persistence.Entities.ProjectComment>().IgnoreBaseProps();
        
        CreateMap<Persistence.Entities.TaskComment, Persistence.Entities.TaskComment>().IgnoreBaseProps();
    }
}