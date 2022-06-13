using NHibernate.Event;
using Todo.Core.Common.Context;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.SessionFactory;

public class AuditEntityListener : IPreInsertEventListener, IPreUpdateEventListener, INhibernateListenerRegistration
{
    private const string SystemId = "System";

    public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return Task.FromResult(false);
        return Task.FromResult(OnPreInsert(@event));
    }

    public bool OnPreInsert(PreInsertEvent @event)
    {
        if (@event.Entity is IAuditableEntity auditEntity)
        {
            var now = DateTime.UtcNow;
            auditEntity.CreatedAt = now;
            auditEntity.ModifiedAt = now;
            auditEntity.Author = auditEntity.Editor = UserContext.UserDisplayName ?? SystemId;
            auditEntity.AuthorId = auditEntity.EditorId = UserContext.UserName ?? SystemId;
        }

        return false;
    }

    public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return Task.FromResult(false);

        return Task.FromResult(OnPreUpdate(@event));
    }

    public bool OnPreUpdate(PreUpdateEvent @event)
    {
        if (@event.Entity is IAuditableEntity auditEntity)
        {
            var now = DateTime.UtcNow;
            auditEntity.ModifiedAt = now;
            auditEntity.Editor = UserContext.UserDisplayName ?? SystemId;
            auditEntity.EditorId = UserContext.UserName ?? SystemId;
        }

        return false;
    }

    public IList<ListenerType> ListernerTypes =>
        new List<ListenerType> {ListenerType.PreInsert, ListenerType.PreUpdate};
}