using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Todo.Core.Common.Configuration;

namespace Todo.Core.Persistence.SessionFactory;

public class SessionFactoryProvider
{
    private readonly IConfigProvider _configProvider;
    private readonly IEnumerable<INhibernateDatabaseConfiguration> _dbConfigs;
    private readonly LoggingInterceptor _loggingInterceptor;
    private readonly IEnumerable<IModelMapperConfiguration> _modelMapperConfigurations;

    public SessionFactoryProvider(IConfigProvider configProvider,
        IEnumerable<INhibernateDatabaseConfiguration> dbConfigs,
        IEnumerable<IModelMapperConfiguration> modelMapperConfigurations, LoggingInterceptor loggingInterceptor)
    {
        _configProvider = configProvider;
        _dbConfigs = dbConfigs;
        _modelMapperConfigurations = modelMapperConfigurations;
        _loggingInterceptor = loggingInterceptor;
    }

    public ISessionFactory CreateFactory()
    {
        var config = new Configuration();
        var beforeMappingCfgs = _dbConfigs.Where(c => !c.AfterMapping);
        foreach (var dbCfg in beforeMappingCfgs) dbCfg.Configure(config);

        var mapping = new ModelMapper();
        foreach (var mcfg in _modelMapperConfigurations) mcfg.ConfigureMapping(mapping);

        config.SetListener(ListenerType.PreInsert, new AuditEntityListener());
        config.SetListener(ListenerType.PreUpdate, new AuditEntityListener());
        config.AddDeserializedMapping(mapping.CompileMappingForAllExplicitlyAddedEntities(), null);
        config.SetInterceptor(_loggingInterceptor);

        var afterMappingCfs = _dbConfigs.Where(c => c.AfterMapping);
        foreach (var cfg in afterMappingCfs) cfg.Configure(config);
        new SchemaUpdate(config).Execute(true, true);
        return config.BuildSessionFactory();
    }
}