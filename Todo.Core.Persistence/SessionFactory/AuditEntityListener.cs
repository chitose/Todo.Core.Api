using NHibernate.Event;
using Todo.Core.Common.Context;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.SessionFactory;

public class AuditEntityListener : IPreInsertEventListener, IPreUpdateEventListener
{
    public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(OnPreInsert(@event));
    }

    public bool OnPreInsert(PreInsertEvent @event)
    {
        if (@event.Entity is IAuditableEntity auditEntity)
        {
            var now = DateTime.UtcNow;
            auditEntity.CreatedAt = now;
            auditEntity.ModifiedAt = now;
            auditEntity.Author = auditEntity.Editor = UserContext.UserName ?? "System";
        }

        return false;
    }

    public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(OnPreUpdate(@event));
    }

    public bool OnPreUpdate(PreUpdateEvent @event)
    {
        if (@event.Entity is IAuditableEntity auditEntity)
        {
            var now = DateTime.UtcNow;
            auditEntity.ModifiedAt = now;
            auditEntity.Editor = UserContext.UserName ?? "System";
        }

        return false;
    }
}