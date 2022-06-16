﻿using NHibernate.Linq;
using Todo.Core.Common.Context;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class ProjectSectionRepository : GenericEntityRepository<ProjectSection>, IProjectSectionRepository
{
    public override Task<ProjectSection?> GetByKey(int key, CancellationToken cancellationToken = default)
    {
        return GetAll()
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken: cancellationToken);
    }

    public override IQueryable<ProjectSection> GetAll()
    {
        return GetUserProjectSectionsOnly();
    }
    
    private IQueryable<ProjectSection> GetUserProjectSectionsOnly()
    {
        return Session.Query<ProjectSection>().Where(s => s.Project.UserProjects.Any(u => u.User.UserName == UserContext.UserName));
    }
}