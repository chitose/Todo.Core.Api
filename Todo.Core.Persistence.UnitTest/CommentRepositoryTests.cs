using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTest;

public class CommentRepositoryTests : BaseTest
{
    private Project? _project;
    private ICommentRepository<ProjectComment>? _projectCommentRepo;
    private TodoTask? _task;
    private ICommentRepository<TaskComment>? _taskCommentRepo;

    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        var projectRepo = _scope.Resolve<IProjectRepository>();
        var taskRepo = _scope.Resolve<ITaskRepository>();
        _projectCommentRepo = _scope.Resolve<ICommentRepository<ProjectComment>>();
        _taskCommentRepo = _scope.Resolve<ICommentRepository<TaskComment>>();
        _project = await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var prj = new Project
            {
                Name = "Test project"
            };

            return projectRepo.Add(prj);
        });

        _task = await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var task = new TodoTask
            {
                Title = "Test task",
                Project = _project
            };
            return taskRepo.Add(task);
        });
    }

    [Test]
    public async Task Add_project_comment_should_work()
    {
        var pc = await CreateProjectComment(_project!, "Hello");
        Assert.IsNotNull(pc);
        Assert.IsTrue(pc.Id > 0);
    }

    [Test]
    public async Task Update_project_comment_should_work()
    {
        var pc = await CreateProjectComment(_project!, "dummy comment");
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            pc.Content = "Updated project comment";
            return _projectCommentRepo!.Save(pc);
        });

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var upc = await _projectCommentRepo!.GetByKey(pc.Id);
            Assert.AreEqual(upc.Content, pc.Content);
        });
    }

    [Test]
    public async Task Delete_project_comment_should_work()
    {
        var pc = await CreateProjectComment(_project!, "dummy comment");
        Assert.IsTrue(pc.Id > 0);
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectCommentRepo!.Delete(pc));

        pc = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectCommentRepo!.GetByKey(pc.Id));
        
        Assert.IsNull(pc);
    }

    private async Task<ProjectComment> CreateProjectComment(Project project, string content)
    {
        return await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var cmt = new ProjectComment
            {
                Content = content,
                Project = project
            };
            return _projectCommentRepo!.Add(cmt);
        });
    }

    [Test]
    public async Task Add_task_comment_should_work()
    {
        var tc = await CreateTaskComment(_task,"hello task");
        Assert.IsNotNull(tc);
        Assert.IsTrue(tc.Id > 0);
    }

    [Test]
    public async Task Update_task_comment_should_work()
    {
        var tc = await CreateTaskComment(_task, "task cmt");
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            tc.Content = "updated cmt";
            return _taskCommentRepo!.Save(tc);
        });

        var utc = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _taskCommentRepo!.GetByKey(tc.Id));
        
        Assert.IsNotNull(utc);
        Assert.AreEqual(utc.Content, tc.Content);
    }

    private async Task<TaskComment> CreateTaskComment(TodoTask? task, string content)
    {
        return await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var cmt = new TaskComment
            {
                Content = content,
                Task = task!
            };
            return _taskCommentRepo!.Add(cmt);
        });
    }
}