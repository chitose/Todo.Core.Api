using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.Tests;

[TestFixture]
public class CommentRepositoryTests : BaseRepositoryTest
{
    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        RestoreExecutionContext();
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
    public async Task Add_project_comment()
    {
        var pc = await _dataCreator.CreateProjectComment(_project!, "Hello");
        pc.Should().NotBeNull();
        pc.Id.Should().BePositive();
    }

    [Test]
    public async Task Update_project_comment()
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
            upc.Content.Should().BeEquivalentTo(pc.Content);
        });
    }

    [Test]
    public async Task Delete_project_comment()
    {
        var pc = await _dataCreator.CreateProjectComment(_project!, "dummy comment");
        pc.Id.Should().BePositive();
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectCommentRepo!.Delete(pc));

        pc = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectCommentRepo!.GetByKey(pc.Id));

        _dataCreator.Remove(pc);

        pc.Should().BeNull();
    }

    [Test]
    public async Task Add_task_comment()
    {
        var tc = await _dataCreator.CreateTaskComment(_task, "hello task");
        tc.Should().NotBeNull();
        tc.Id.Should().BePositive();
    }

    [Test]
    public async Task Update_task_comment()
    {
        var tc = await _dataCreator.CreateTaskComment(_task, "task cmt");
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            tc.Content = "updated cmt";
            return _taskCommentRepo!.Save(tc);
        });

        var utc = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _taskCommentRepo!.GetByKey(tc.Id));

        utc.Should().NotBeNull();
        utc.Content.Should().BeEquivalentTo(tc.Content);
    }
}