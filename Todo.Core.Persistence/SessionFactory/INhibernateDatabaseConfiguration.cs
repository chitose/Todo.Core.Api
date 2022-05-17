namespace Todo.Core.Persistence.SessionFactory;

public interface INhibernateDatabaseConfiguration
{
    public bool AfterMapping { get; }
    void Configure(NHibernate.Cfg.Configuration config);
}