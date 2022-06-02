using NHibernate.Linq;
using Todo.Core.Common.Context;
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
    
    public override Task<Project> GetByKey(int key, CancellationToken cancellationToken = default)
    {
        return GetUserProjectOnly()
            .Fetch(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public override IQueryable<Project> GetAll()
    {
        return GetUserProjectOnly();
    }

    public override async Task<Project> Add(Project entity, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindByUserName(UserContext.UserName);
        entity.Users.Add(user);
        return await base.Add(entity, cancellationToken);
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
        return Session.Query<Project>().Where(p => p.Users.Any(u => u.UserName == UserContext.UserName));
    }
}