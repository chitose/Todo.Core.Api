using NHibernate.Linq;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Exceptions;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.Extensions;

public static class RepositoryExtensions
{
    public static async Task<int> CalculateNewEntityOrder<T>(this IGenericRepository<T> repo, int? above = null,
        int? below = null, CancellationToken cancellationToken = default) where T : BaseEntity, IOrderable, new()
    {
        var order = 0;
        if (above.HasValue || below.HasValue)
        {
            var target = await
                repo.GetByKey(above ?? below!.Value, cancellationToken);

            var otherEntitiesQuery = repo.GetAll();
            if (above.HasValue)
            {
                order = target.Order - 1;
                otherEntitiesQuery = otherEntitiesQuery.Where(x => x.Order < target.Order);
            }
            else
            {
                order = target.Order + 1;
                otherEntitiesQuery = otherEntitiesQuery.Where(x => x.Order > target.Order);
            }

            var others = await otherEntitiesQuery.ToListAsync(cancellationToken);
            others.ForEach(p =>
            {
                p.Order += (above.HasValue ? -1 : 1) * 1;
                repo.Save(p, cancellationToken);
            });
        }
        else
        {
            var maxdbOrder = (await repo.GetAll().OrderByDescending(x => x.Order).FirstOrDefaultAsync())?.Order ?? 0;
            order = maxdbOrder + 1;
        }

        return order;
    }

    public static async Task SwapEntityOrder<T>(this IGenericRepository<T> repo, int source, int target,
        CancellationToken cancellationToken = default)
        where T : BaseEntity, IOrderable, new()
    {
        var projects = await repo.GetAll()
            .Where(x => x.Id == source || x.Id == target).ToListAsync(cancellationToken);

        var sp = projects.FirstOrDefault(p => p.Id == source);
        var tp = projects.FirstOrDefault(p => p.Id == target);
        var obj = new T();
        if (sp == null)
        {
            switch (obj)
            {
                case Project _:
                    throw new ProjectNotFoundException(source);
                case ProjectSection _:
                    throw new SectionNotFoundException(source);
            }
        }

        if (tp == null)
        {
            switch (obj)
            {
                case Project _:
                    throw new ProjectNotFoundException(target);
                case ProjectSection _:
                    throw new SectionNotFoundException(target);
            }
        }

        (sp!.Order, tp!.Order) = (tp.Order, sp.Order);

        await repo.Save(sp, cancellationToken);
        await repo.Save(tp, cancellationToken);
    }
}