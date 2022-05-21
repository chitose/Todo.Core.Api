using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public interface ICommentRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseComment, new()
{
}