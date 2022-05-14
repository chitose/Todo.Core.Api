using Autofac;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.Mapping;

public class MappingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MappingConfiguration>().As<IModelMapperConfiguration>();
    }
}