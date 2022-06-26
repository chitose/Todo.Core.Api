using Autofac;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Todo.Core.Common.Tests;
using Todo.Core.Domain.Enum;
using Todo.Core.Domain.Project;

namespace Todo.Core.Service.Tests;

[TestFixture]
public class MappingTests : BaseTest
{
    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        using var scope = _container.BeginLifetimeScope();
        _mapper = scope.Resolve<IMapper>();
    }

    private IMapper _mapper;

    [Test]
    public void ProjectUpdateInfo_to_Project()
    {
        var prjUpdateInfo = new ProjectUpdateInfo
        {
            Name = "Update name only",
            View = ProjectView.Dashboard
        };

        var prj = new Persistence.Entities.Project
        {
            Name = "Project name",
            Archived = false,
            View = ProjectView.List
        };

        _mapper.Map(prjUpdateInfo, prj);

        prj.Name.Should().BeEquivalentTo(prjUpdateInfo.Name);
        prj.Archived.Should().BeFalse();
        prj.View.Should().Be(prjUpdateInfo.View);
    }
}