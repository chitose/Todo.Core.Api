using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTests;

[TestFixture]
public class TaskRepositoryTests : BaseTest
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
        Assert.IsTrue(task.Id > 0);

        var sect = await _dataCreator.CreateProjectSection(prj, "Test sect");
        var taskInSect = await _dataCreator.CreateTask(prj, "Task in section", sect);
        Assert.IsTrue(taskInSect.Id > 0);

        var childTask = await _dataCreator.CreateTask(prj, "child task", parent: task);
        Assert.IsTrue(childTask.Id > 0);
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
            Assert.AreEqual(ut.Title, task.Title);
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

        Assert.IsNull(task);
    }
}