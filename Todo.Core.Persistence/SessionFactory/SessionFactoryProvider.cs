using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using Todo.Core.Common.Configuration;

namespace Todo.Core.Persistence.SessionFactory;

public class SessionFactoryProvider
{
    private readonly IConfigProvider _configProvider;
    private readonly INhibernateDatabaseConfiguration _dbConfig;
    private readonly IEnumerable<IModelMapperConfiguration> _modelMapperConfigurations;
    private readonly LoggingInterceptor _loggingInterceptor;
    public SessionFactoryProvider(IConfigProvider configProvider, INhibernateDatabaseConfiguration dbConfig,
        IEnumerable<IModelMapperConfiguration> modelMapperConfigurations, LoggingInterceptor loggingInterceptor)
    {
        _configProvider = configProvider;
        _dbConfig = dbConfig;
        _modelMapperConfigurations = modelMapperConfigurations;
        _loggingInterceptor = loggingInterceptor;
    }
    public ISessionFactory CreateFactory()
    {
        var config = new Configuration();
        _dbConfig.Configure(config);
        var mapping = new ModelMapper();
        foreach (var mcfg in _modelMapperConfigurations)
        {
            mcfg.ConfigureMapping(mapping);
        }

        config.SetListener(ListenerType.PreInsert, new AuditEntityListener());
        config.SetListener(ListenerType.PreUpdate, new AuditEntityListener());
        config.AddDeserializedMapping(mapping.CompileMappingForAllExplicitlyAddedEntities(), null);
        config.SetInterceptor(_loggingInterceptor);
        new NHibernate.Tool.hbm2ddl.SchemaUpdate(config).Execute(true, true);
        return config.BuildSessionFactory();
    }
}