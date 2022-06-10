using NHibernate.Linq;
using Todo.Core.Common.Context;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Exceptions;

namespace Todo.Core.Persistence.Repositories;

public class ProjectRepository : GenericEntityRepository<Project>, IProjectRepository
{
    private readonly IUserRepository _userRepository;

    public ProjectRepository(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<Project?> GetByKey(int key, CancellationToken cancellationToken = default)
    {
        // work-around for issue with Fetch and FirstOrDefault
        // see https://github.com/nhibernate/nhibernate-core/issues/1141
        var result = await GetUserProjectOnly()
            .Fetch(x => x.UserProjects)
            .Where(x => x.Id == key)
            .ToListAsync(cancellationToken);
        return result.FirstOrDefault();
    }

    public override IQueryable<Project> GetAll()
    {
        return GetUserProjectOnly();
    }

    public override async Task<Project> Add(Project entity, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindByUserName(UserContext.UserName);
        await base.Add(entity, cancellationToken);
        var up = new UserProject
        {
            User = user,
            Project = entity,
            Owner = true,
            JoinedTime = DateTime.UtcNow
        };
        await UnitOfWork.Current.GetCurrentSession().PersistAsync(up, cancellationToken);
        entity.UserProjects.Add(up);
        return entity;
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
        return Session.Query<Project>().Where(p => p.UserProjects.Any(u => u.User.UserName == UserContext.UserName));
    }
}