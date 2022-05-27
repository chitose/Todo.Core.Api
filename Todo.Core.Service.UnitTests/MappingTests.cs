using Autofac;
using AutoMapper;
using NUnit.Framework;
using Todo.Core.Domain.Enum;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.UnitTests;

namespace Todo.Core.Service.UnitTests;

[TestFixture]
public class MappingTests : BaseTest
{
    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _mapper = _scope.Resolve<IMapper>();
    }

    private IMapper _mapper;

    [Test]
    public void ProjectUpdateInfo_to_Project_should_work()
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

        Assert.AreEqual(prj.Name, prjUpdateInfo.Name);
        Assert.IsTrue(prj.Archived == false);
        Assert.AreEqual(prj.View, prjUpdateInfo.View);
    }
}