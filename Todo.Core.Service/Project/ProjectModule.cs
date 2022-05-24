using Autofac;

namespace Todo.Core.Service.Project;

public class ProjectModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ProjectService>().As<IProjectService>();
    }
}