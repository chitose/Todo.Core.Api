using NHibernate.Linq;
using Todo.Core.Common.Extensions;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class TaskRepository : GenericEntityRepository<TodoTask>, ITaskRepository
{
    private readonly ICommentRepository<TaskComment> _taskCommentRepo;

    public TaskRepository(ICommentRepository<TaskComment> taskCommentRep)
    {
        _taskCommentRepo = taskCommentRep;
    }

    public async Task<TodoTask> CloneTask(TodoTask todoTask, Project? project, ProjectSection? section,
        TodoTask? parentTask,
        CancellationToken cancellationToken)
    {
        var sameSectClone = todoTask.Section?.Id == section?.Id;
        var newTask = new TodoTask
        {
            Project = project ?? section?.Project ?? todoTask.Project,
            Section = section ?? todoTask.Section,
            Title = sameSectClone ? todoTask.Title.AddCloneSuffix() : todoTask.Title,
            Description = todoTask.Description,
            Priority = todoTask.Priority,
            AssignedTo = todoTask.AssignedTo,
            ParentTask = parentTask,
            DueDate = todoTask.DueDate,
            Labels = new List<Label>()
        };

        foreach (var lbl in todoTask.Labels) newTask.Labels.Add(lbl);

        foreach (var cmt in todoTask.Comments)
        {
            var tcmt = await _taskCommentRepo.Add(new TaskComment
            {
                Content = cmt.Content,
                Task = newTask
            }, cancellationToken);
            newTask.Comments.Add(tcmt);
        }

        newTask = await Add(newTask, cancellationToken);

        var subTasks = await GetQuery().Where(x => x.ParentTask.Id == todoTask.Id)
            .ToListAsync(cancellationToken);
        foreach (var st in subTasks) await CloneTask(st, newTask.Project, newTask.Section, newTask, cancellationToken);

        return newTask;
    }
}