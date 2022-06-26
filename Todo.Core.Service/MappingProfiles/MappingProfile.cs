using AutoMapper;
using Todo.Core.Common.Extensions;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Extensions;

namespace Todo.Core.Service.MappingProfiles;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectUpdateInfo, Persistence.Entities.Project>().IgnoreNullProperties();

        CreateMap<Persistence.Entities.Project, Persistence.Entities.Project>().IgnoreBaseProps()
            .ForMember(x => x.Tasks, opt => opt.Ignore())
            .ForMember(x => x.Sections, opt => opt.Ignore())
            .ForMember(x => x.Comments, opt => opt.Ignore())
            .ForMember(x => x.Labels, opt => opt.Ignore())
            .ForMember(x => x.UserProjects, opt => opt.Ignore());
        ;

        CreateMap<ProjectSection, ProjectSection>().IgnoreBaseProps()
            .ForMember(x => x.Tasks, opt => opt.Ignore());

        CreateMap<Persistence.Entities.TodoTask, Persistence.Entities.TodoTask>().IgnoreBaseProps()
            .ForMember(x => x.Labels, opt => opt.Ignore())
            .ForMember(x => x.Comments, opt => opt.Ignore())
            .ForMember(x => x.SubTasks, opt => opt.Ignore());

        CreateMap<ProjectComment, ProjectComment>().IgnoreBaseProps();

        CreateMap<TaskComment, TaskComment>().IgnoreBaseProps();

        CreateMap<TaskCreationInfo, Persistence.Entities.TodoTask>()
            .ForMember(x => x.Project, opt => opt.Ignore())
            .ForMember(x => x.Section, opt => opt.Ignore())
            .ForMember(x => x.ParentTask, opt => opt.Ignore());
    }
}