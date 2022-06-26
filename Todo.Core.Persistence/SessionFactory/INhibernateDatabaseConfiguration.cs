using NHibernate.Cfg;

namespace Todo.Core.Persistence.SessionFactory;

public interface INhibernateDatabaseConfiguration
{
    public bool AfterMapping { get; }

    public string ConnectionType { get; }
    void Configure(Configuration config);
}