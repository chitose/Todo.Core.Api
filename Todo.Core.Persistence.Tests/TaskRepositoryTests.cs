using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using Todo.Core.Common.Tests;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.Tests;

[TestFixture]
public class TaskRepositoryTests : BaseRepositoryTest
{
    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _taskRepository = _scope.Resolve<ITaskRepository>();
    }

    private ITaskRepository _taskRepository;

    [Test]
    public async Task Task_creation_should_work()
    {
        var prj = await _dataCreator.CreateProject("Test project");
        var task = await _dataCreator.CreateTask(prj, "Test task");

        task.Should().NotBeNull();
        task.Id.Should().BePositive();

        var sect = await _dataCreator.CreateProjectSection(prj, "Test sect");
        var taskInSect = await _dataCreator.CreateTask(prj, "Task in section", sect);
        taskInSect.Should().NotBeNull();
        taskInSect.Id.Should().BePositive();

        var childTask = await _dataCreator.CreateTask(prj, "child task", parent: task);
        childTask.Should().NotBeNull();
        childTask.Id.Should().BePositive();
    }

    [Test]
    public async Task Task_update_should_work()
    {
        var prj = await _dataCreator.CreateProject("Test project");
        var task = await _dataCreator.CreateTask(prj, "Test task");

        await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            task.Title = "Updated title";
            return _taskRepository.Save(task);
        });

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var ut = await _taskRepository.GetByKey(task.Id);
            ut.Title.Should().BeEquivalentTo(task.Title);
        });
    }

    [Test]
    public async Task Task_deletion_should_work()
    {
        var prj = await _dataCreator.CreateProject("Test project");
        var task = await _dataCreator.CreateTask(prj, "Test task");
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _taskRepository.Delete(task));

        _dataCreator.Remove(task);
        task = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _taskRepository.GetByKey(task.Id));

        task.Should().BeNull();
    }
}