using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.Tests;

[TestFixture]
public class ProjectSectionTests : BaseRepositoryTest
{
    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _projectSectionRepository = _scope.Resolve<IProjectSectionRepository>();
    }

    private IProjectSectionRepository _projectSectionRepository;

    [Test]
    public async Task Project_section_creation()
    {
        var project = await _dataCreator.CreateProject("Test project");
        var section = await _dataCreator.CreateProjectSection(project, "Test section");

        section.Should().NotBeNull();
        section.Id.Should().BePositive();
    }

    [Test]
    public async Task Project_section_update()
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
            usect.Title.Should().BeEquivalentTo(sect.Title);
        });
    }

    [Test]
    public async Task Project_section_deletion()
    {
        var project = await _dataCreator.CreateProject("Test project");
        var section = await _dataCreator.CreateProjectSection(project, "Test section");

        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _projectSectionRepository.Delete(section));

        _dataCreator.Remove(section);

        section = await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
            _projectSectionRepository.GetByKey(section.Id));
        section.Should().BeNull();
    }
}