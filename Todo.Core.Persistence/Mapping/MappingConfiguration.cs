using NHibernate.Mapping.ByCode;
using Todo.Core.Persistence.SessionFactory;

namespace Todo.Core.Persistence.Mapping;

public class MappingConfiguration : IModelMapperConfiguration
{
    public void ConfigureMapping(ModelMapper mapper)
    {
        mapper.AddMapping(new BaseCommentMapping());
        mapper.AddMapping(new ProjectCommentMapping());
        mapper.AddMapping(new ProjectMapping());
        mapper.AddMapping(new UserMapping());
    }
}