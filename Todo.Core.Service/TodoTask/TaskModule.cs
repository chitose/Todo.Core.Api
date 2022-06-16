using Autofac;

namespace Todo.Core.Service.TodoTask;

public class TaskModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TodoTaskService>().As<ITodoTaskService>();
    }
}