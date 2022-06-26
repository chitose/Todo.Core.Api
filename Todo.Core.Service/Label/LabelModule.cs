using Autofac;

namespace Todo.Core.Service.Label;

public class LabelModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LabelService>().As<ILabelService>();
    }
}