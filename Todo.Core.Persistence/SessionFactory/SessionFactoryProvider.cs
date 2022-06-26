using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Todo.Core.Common.Configuration;
using Todo.Core.Common.Extensions;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.SessionFactory;

public class SessionFactoryProvider
{
    private readonly IConfigProvider _configProvider;
    private readonly IEnumerable<INhibernateDatabaseConfiguration> _dbConfigs;
    private readonly IEnumerable<INhibernateListenerRegistration> _listenerRegistrations;
    private readonly LoggingInterceptor _loggingInterceptor;
    private readonly IEnumerable<IModelMapperConfiguration> _modelMapperConfigurations;

    public SessionFactoryProvider(IConfigProvider configProvider,
        IEnumerable<INhibernateDatabaseConfiguration> dbConfigs,
        IEnumerable<IModelMapperConfiguration> modelMapperConfigurations, LoggingInterceptor loggingInterceptor,
        IEnumerable<INhibernateListenerRegistration> nhibernateListenerRegistrations)
    {
        _configProvider = configProvider;
        _dbConfigs = dbConfigs;
        _modelMapperConfigurations = modelMapperConfigurations;
        _loggingInterceptor = loggingInterceptor;
        _listenerRegistrations = nhibernateListenerRegistrations;
    }

    public ISessionFactory CreateFactory()
    {
        var config = new Configuration();
        var connectionType = _configProvider.GetConnectionType();
        if (string.IsNullOrEmpty(connectionType)) throw new InvalidOperationException("Missing connection type value");

        var targetConfigs = _dbConfigs
            .Where(x => string.IsNullOrEmpty(x.ConnectionType) || x.ConnectionType == connectionType).ToList();

        if (!targetConfigs.Any())
            throw new InvalidOperationException(
                $"No {nameof(INhibernateDatabaseConfiguration)} bindings found for connection type {connectionType}");
        var beforeMappingCfgs = targetConfigs.Where(c => !c.AfterMapping);
        foreach (var dbCfg in beforeMappingCfgs) dbCfg.Configure(config);

        var mapping = new ModelMapper();
        foreach (var mcfg in _modelMapperConfigurations) mcfg.ConfigureMapping(mapping);

        foreach (var lr in _listenerRegistrations)
        foreach (var lt in lr.ListernerTypes)
            config.SetListener(lt, lr);

        mapping.AfterMapBag += (inspector, member, customizer) =>
        {
            if (member.LocalMember.DeclaringType == typeof(Project))
                if (member.LocalMember.Name != nameof(Project.Labels))
                    customizer.Key(k => k.OnDelete(OnDeleteAction.Cascade));

            if (member.LocalMember.DeclaringType == typeof(ProjectSection))
                if (member.LocalMember.Name == nameof(ProjectSection.Tasks))
                    customizer.Key(k => k.OnDelete(OnDeleteAction.Cascade));

            if (member.LocalMember.DeclaringType == typeof(User))
                if (member.LocalMember.Name != nameof(User.Tasks))
                    customizer.Key(k => k.OnDelete(OnDeleteAction.Cascade));
        };

        config.AddDeserializedMapping(mapping.CompileMappingForAllExplicitlyAddedEntities(), null);
        config.SetInterceptor(_loggingInterceptor);

        var afterMappingCfs = targetConfigs.Where(c => c.AfterMapping);
        foreach (var cfg in afterMappingCfs) cfg.Configure(config);

        new SchemaUpdate(config).Execute(true, true);
        return config.BuildSessionFactory();
    }
}