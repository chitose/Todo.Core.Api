using NHibernate;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public abstract class GenericReadonlyRepository<TEntity> : IGenericReadonlyRepository<TEntity> where TEntity : BaseEntity
{
    protected IStatelessSession Session => StatelessUnitOfWork.Current?.GetCurrentSession();


    public IQueryable<TEntity> Query()
    {
        return Session.Query<TEntity>();
    }
}