using NHibernate.Linq;
using Todo.Core.Common.Context;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Exceptions;

namespace Todo.Core.Persistence.Repositories;

public class ProjectRepository : GenericEntityRepository<Project>, IProjectRepository
{
    public override Task<Project> GetByKey(int key, CancellationToken cancellationToken = default)
    {
        return GetUserProjectOnly().Where(x => x.Id == key).FirstOrDefaultAsync(cancellationToken);
    }

    public override IQueryable<Project> GetAll()
    {
        return GetUserProjectOnly();
    }

    public override async Task DeleteByKey(int key, CancellationToken cancellationToken = default)
    {
        var prj = await GetByKey(key, cancellationToken);
        if (prj == null)
        {
            throw new ProjectNotFoundException(key);
        }

        await base.DeleteByKey(key, cancellationToken);
    }

    private IQueryable<Project> GetUserProjectOnly()
    {
        return Session.Query<Project>().Where(p => p.Users.Any(u => u.Id == UserContext.UserId));
    }
}