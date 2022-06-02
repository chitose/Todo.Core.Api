using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Common.Context;
using Todo.Core.Domain.Enum;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.UnitTests;
using Todo.Core.Service.Project;

namespace Todo.Core.Service.UnitTests;

[TestFixture]
public class ProjectServiceTests : BaseTest
{
    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        RestoreExecutionContext();
        _projectService = _scope.Resolve<IProjectService>();
        _anchorProject = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "Anchor project"
        });
    }

    [SetUp]
    public new async Task Setup()
    {
        RestoreExecutionContext();
        // update anchor project order
        _anchorProject = await _projectService.GetProject(_anchorProject.Id);
    }

    private Persistence.Entities.Project _anchorProject;

    private IProjectService _projectService;

    [Test]
    public async Task Create_project_should_work()
    {
        var prjCreationInfo = new ProjectCreationInfo
        {
            Name = "Test project",
            View = ProjectView.Dashboard,
            Archived = false,
            Default = true,
            GroupBy = "",
            SortAsc = false,
            SortBy = "",
            ShowCompleted = false
        };
        var prj = await _projectService.CreateProject(prjCreationInfo);
        Assert.IsNotNull(prj);
        Assert.AreEqual(prj.Name, prjCreationInfo.Name);
        Assert.AreEqual(prj.View, prjCreationInfo.View);
        Assert.IsTrue(prj.Order > _anchorProject.Order);
    }

    [Test]
    public async Task Create_above_below_project_should_work()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "above project",
            AboveProject = _anchorProject.Id
        });

        Assert.IsTrue(prj.Order < _anchorProject.Order);

        var belowPrj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "below project",
            BelowProject = _anchorProject.Id
        });

        Assert.IsTrue(belowPrj.Order > _anchorProject.Order);
    }

    [Test]
    public async Task Update_project_should_work()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "Test project"
        });

        prj = await _projectService.UpdateProject(prj.Id, new ProjectUpdateInfo
        {
            Name = "Update name",
            View = ProjectView.List
        });

        Assert.AreEqual(prj.Name, "Update name");
        Assert.AreEqual(prj.View, ProjectView.List);
    }

    [Test]
    public async Task Delete_project_should_work()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "Test project"
        });

        Assert.IsTrue(prj.Id > 0);
        await _projectService.DeleteProject(prj.Id);

        prj = await _projectService.GetProject(prj.Id);

        Assert.IsNull(prj);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task Get_projects_should_work(bool isArchived)
    {
        await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "test project 1",
            Archived = isArchived
        });

        await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "test project 2",
            Archived = isArchived
        });

        var projects = await _projectService.GetProjects(isArchived);

        Assert.IsTrue(projects.Count >= 2);
        Assert.IsTrue(projects.All(p => p.Archived == isArchived));
    }

    [Test]
    public async Task Swap_projects_order_should_work()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "above anchor",
            AboveProject = _anchorProject.Id
        });

        var prj1 = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "below anchor",
            BelowProject = _anchorProject.Id
        });

        await _projectService.SwapProjectOrder(prj.Id, prj1.Id);

        var uprj = await _projectService.GetProject(prj.Id);
        var uprj1 = await _projectService.GetProject(prj1.Id);
        Assert.AreEqual(prj.Order, uprj1.Order);
        Assert.AreEqual(uprj.Order, prj1.Order);
    }

    [Test]
    public async Task Add_comment_should_work()
    {
        var cmt = await _projectService.AddComment(_anchorProject.Id,
            "Hello");

        Assert.IsNotNull(cmt);
        Assert.IsTrue(cmt.Id > 0);
        Assert.IsTrue(cmt.AuthorId == UserContext.UserName);
    }

    [Test]
    public async Task Load_comments_should_work()
    {
        var cmt = await _projectService.AddComment(_anchorProject.Id,
            $"Hello from {_user1.DisplayName}");
        SwitchUser(_user2);
        var cmt1 = await _projectService.AddComment(_anchorProject.Id,
            $"Hello from {_user2.DisplayName}");

        var cmts = await _projectService.LoadComments(_anchorProject.Id);

        Assert.IsTrue(cmts.Exists(c => c.Id == cmt.Id
                                       || c.Id == cmt1.Id));
    }

    [Test]
    public async Task Invite_user_should_work()
    {
        await _projectService.InviteUserToProject(_anchorProject.Id, _user2.UserName);
        _anchorProject = await _projectService.GetProject(_anchorProject.Id);
        Assert.IsTrue(_anchorProject.Users.Any(u => u.Id == _user2.Id));
    }

    #region Sections

    

    #endregion
}