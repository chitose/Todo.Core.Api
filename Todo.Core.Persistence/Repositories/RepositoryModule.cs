using Autofac;

namespace Todo.Core.Persistence.Repositories;

public class RepositoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(GenericEntityRepository<>))
            .As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(GenericReadonlyRepository<>))
            .As(typeof(IGenericReadonlyRepository<>)).InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(CommentRepository<>))
            .As(typeof(ICommentRepository<>)).InstancePerLifetimeScope();

        builder.RegisterType<UserRepository>().As<IUserRepository>();
        builder.RegisterType<ProjectRepository>().As<IProjectRepository>();
        builder.RegisterType<LabelRepository>().As<ILabelRepository>();
        builder.RegisterType<TaskRepository>().As<ITaskRepository>();
        builder.RegisterType<ProjectSectionRepository>().As<IProjectSectionRepository>();
    }
}