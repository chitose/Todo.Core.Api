using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class CommentRepository<TEntity> : GenericEntityRepository<TEntity>, ICommentRepository<TEntity>
    where TEntity : BaseComment, new()
{
}