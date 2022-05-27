using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTests;

public class DataCreator
{
    private readonly IList<BaseEntity> _createdEntities = new List<BaseEntity>();
    private readonly ILabelRepository _labelRepository;
    private readonly ICommentRepository<ProjectComment> _projectCommentRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectSectionRepository _projectSectionRepository;
    private readonly ICommentRepository<TaskComment> _taskCommentRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWorkProvider _unitOfWorkProvider;
    private readonly IUserRepository _userRepository;

    public DataCreator(ILifetimeScope scope)
    {
        _unitOfWorkProvider = scope.Resolve<IUnitOfWorkProvider>();
        _projectRepository = scope.Resolve<IProjectRepository>();
        _projectSectionRepository = scope.Resolve<IProjectSectionRepository>();
        _taskRepository = scope.Resolve<ITaskRepository>();
        _labelRepository = scope.Resolve<ILabelRepository>();
        _projectCommentRepository = scope.Resolve<ICommentRepository<ProjectComment>>();
        _taskCommentRepository = scope.Resolve<ICommentRepository<TaskComment>>();
        _userRepository = scope.Resolve<IUserRepository>();
    }

    public void Remove(BaseEntity entity)
    {
        _createdEntities.Remove(entity);
    }

    public Task<Project> CreateProject(string name)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var prj = new Project { Name = name };
            _createdEntities.Add(prj);
            return _projectRepository.Add(prj);
        });
    }

    public Task<ProjectSection> CreateProjectSection(Project prj, string name)
    {
        var sect = new ProjectSection
        {
            Title = name, Project = prj
        };
        _createdEntities.Add(sect);
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectSectionRepository.Add(
            sect));
    }

    public Task<Label> CreateLabel(string name)
    {
        var lbl = new Label { Title = name };
        _createdEntities.Add(lbl);
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() => _labelRepository.Add(lbl));
    }

    public Task<ProjectComment> CreateProjectComment(Project project, string content)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var cmt = new ProjectComment
            {
                Content = content,
                Project = project
            };
            _createdEntities.Add(cmt);
            return _projectCommentRepository!.Add(cmt);
        });
    }

    public Task<TaskComment> CreateTaskComment(TodoTask? task, string content)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var cmt = new TaskComment
            {
                Content = content,
                Task = task!
            };
            _createdEntities.Add(cmt);
            return _taskCommentRepository!.Add(cmt);
        });
    }

    public Task<TodoTask> CreateTask(Project project, string testTask,
        ProjectSection? section = null, TodoTask? parent = null)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var task = new TodoTask
            {
                Title = testTask,
                Section = section,
                Project = project,
                ParentTask = parent
            };
            _createdEntities.Add(task);
            return _taskRepository.Add(task);
        });
    }
}