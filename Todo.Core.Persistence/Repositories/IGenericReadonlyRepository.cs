using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface IGenericReadonlyRepository<TEntity> where TEntity: BaseEntity
{
    IQueryable<TEntity> Query();
}