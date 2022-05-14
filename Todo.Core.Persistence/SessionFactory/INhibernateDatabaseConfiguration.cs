namespace Todo.Core.Persistence.SessionFactory;

public interface INhibernateDatabaseConfiguration
{
    void Configure(NHibernate.Cfg.Configuration config);
}