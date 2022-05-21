using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTest;

[TestFixture]
public class CommentRepositoryTests : BaseTest
{
    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        _projectCommentRepo = _scope.Resolve<ICommentRepository<ProjectComment>>();
        _taskCommentRepo = _scope.Resolve<ICommentRepository<TaskComment>>();
        _project = await _dataCreator.CreateProject("Test project");
        _task = await _dataCreator.CreateTask(_project, "Test task");
    }

    private Project? _project;
    private ICommentRepository<ProjectComment>? _projectCommentRepo;
    private TodoTask? _task;
    private ICommentRepository<TaskComment>? _taskCommentRepo;

    [Test]
    public async Task Add_project_comment_should_work()
    {
        var pc = await _dataCreator.CreateProjectComment(_project!, "Hello");
        Assert.IsNotNull(pc);
        Assert.IsTrue(pc.Id > 0);
    }

    [Test]
    public async Task Update_project_comment_should_work()
    {
        var pc = await _dataCreator.CreateProjectComment(_project!, "dummy comment");
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
        var pc = await _dataCreator.CreateProjectComment(_project!, "dummy comment");
        Assert.IsTrue(pc.Id > 0);
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectCommentRepo!.Delete(pc));

        pc = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectCommentRepo!.GetByKey(pc.Id));

        _dataCreator.Remove(pc);

        Assert.IsNull(pc);
    }

    [Test]
    public async Task Add_task_comment_should_work()
    {
        var tc = await _dataCreator.CreateTaskComment(_task, "hello task");
        Assert.IsNotNull(tc);
        Assert.IsTrue(tc.Id > 0);
    }

    [Test]
    public async Task Update_task_comment_should_work()
    {
        var tc = await _dataCreator.CreateTaskComment(_task, "task cmt");
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            tc.Content = "updated cmt";
            return _taskCommentRepo!.Save(tc);
        });

        var utc = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _taskCommentRepo!.GetByKey(tc.Id));

        Assert.IsNotNull(utc);
        Assert.AreEqual(utc.Content, tc.Content);
    }
}