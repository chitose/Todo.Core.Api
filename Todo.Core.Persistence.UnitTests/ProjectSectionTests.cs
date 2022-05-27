using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTests;

[TestFixture]
public class ProjectSectionTests : BaseTest
{
    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _projectSectionRepository = _scope.Resolve<IProjectSectionRepository>();
    }

    private IProjectSectionRepository _projectSectionRepository;

    [Test]
    public async Task Project_section_creation_should_work()
    {
        var project = await _dataCreator.CreateProject("Test project");
        var section = await _dataCreator.CreateProjectSection(project, "Test section");

        Assert.IsTrue(section.Id > 0);
    }

    [Test]
    public async Task Project_section_update_should_work()
    {
        var prj = await _dataCreator.CreateProject("Test Project");
        var sect = await _dataCreator.CreateProjectSection(prj, "Test section");
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            sect.Title = "Updated section name";
            return _projectSectionRepository.Save(sect);
        });

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var usect = await _projectSectionRepository.GetByKey(sect.Id);
            Assert.AreEqual(usect.Title, sect.Title);
        });
    }

    [Test]
    public async Task Project_section_deletion_should_work()
    {
        var project = await _dataCreator.CreateProject("Test project");
        var section = await _dataCreator.CreateProjectSection(project, "Test section");

        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectSectionRepository.Delete(section));

        _dataCreator.Remove(section);

        section = await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
            _projectSectionRepository.GetByKey(section.Id));
        Assert.IsNull(section);
    }
}