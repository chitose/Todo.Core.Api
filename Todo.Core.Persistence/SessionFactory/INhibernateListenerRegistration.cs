using NHibernate.Event;

namespace Todo.Core.Persistence.SessionFactory;

public interface INhibernateListenerRegistration
{
    IList<ListenerType> ListernerTypes { get; }
}