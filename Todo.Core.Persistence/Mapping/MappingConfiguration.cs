using NHibernate.Mapping.ByCode;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.Mapping;

public class MappingConfiguration : IModelMapperConfiguration
{
    public void ConfigureMapping(ModelMapper mapper)
    {
        mapper.AddMapping<BaseCommentMapping>();
        mapper.AddMapping<ProjectCommentMapping>();
        mapper.AddMapping<ProjectMapping>();
        mapper.AddMapping<UserMapping>();
    }
}