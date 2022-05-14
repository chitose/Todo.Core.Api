using NHibernate.Mapping.ByCode;

namespace Todo.Core.Persistence.SessionFactory;

public interface IModelMapperConfiguration
{
    void ConfigureMapping(ModelMapper mapper);
}