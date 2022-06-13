using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using Todo.Core.Common.Context;
using Todo.Core.Common.Exception;
using Todo.Core.Common.Tests;
using Todo.Core.Domain.Enum;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Exceptions;
using Todo.Core.Service.Project;

namespace Todo.Core.Service.Tests;

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

    private Persistence.Entities.Project? _anchorProject;

    private IProjectService _projectService;

    [Test]
    public async Task Create_project()
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
    public async Task Cannot_create_project_without_name()
    {
        Func<Task> act = async () => await _projectService.CreateProject(new ProjectCreationInfo());

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Create_above_below_project()
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
    public async Task Update_project()
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
    public async Task Collaborator_can_update_project()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "Project"
        });

        await _projectService.InviteUserToProject(prj.Id, _user2.UserName);

        await RunWithContextOfUser(_user2, async () =>
          {
            const string updateProjectName = "Update from user 2";
            Func<Task> act = async () => await _projectService.UpdateProject(prj.Id, new ProjectUpdateInfo
            {
                Name = updateProjectName
            });

            await act.Should().NotThrowAsync<ProjectNotFoundException>();
            prj = await _projectService.GetProject(prj.Id);
            prj.Name.Should().BeEquivalentTo(updateProjectName);
        });
    }

    [Test]
    public async Task Only_project_collaborator_can_see_project()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "test"
        });

        await RunWithContextOfUser(_user2, async () =>
        {
            Func<Task<Persistence.Entities.Project>> act = async () => await _projectService.GetProject(prj.Id);
            (await act.Should().NotThrowAsync()).Which.Should().BeNull();
        });
    }

    [Test]
    public async Task Delete_own_project()
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
    public async Task Cannot_delete_other_project()
    {
        var user2Prj = await RunWithContextOfUser(_user2, () => _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "User 2 project"
        }));

        Func<Task> act = async () => await _projectService.DeleteProject(user2Prj.Id);

        await act.Should().ThrowAsync<ProjectNotFoundException>();
    }

    [Test]
    public async Task Only_owner_can_delete_project()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "User 1 project"
        });

        prj.AuthorId.Should().Be(UserContext.UserName);

        await _projectService.InviteUserToProject(prj.Id, _user2.UserName);

        await RunWithContextOfUser(_user2, async () =>
        {
            UserContext.UserName.Should().Be(_user2.UserName);
            var act = async () => await _projectService.DeleteProject(prj.Id);
            await act.Should().ThrowAsync<TodoException>()
                .WithMessage("Only project owner can delete the project");
        });
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task Get_projects(bool isArchived)
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
    public async Task Swap_projects_order()
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
    public async Task Add_comment()
    {
        var cmt = await _projectService.AddComment(_anchorProject!.Id,
            "Hello");

        cmt.Should().NotBeNull();
        cmt.Id.Should().BePositive();
        cmt.AuthorId.Should().Be(UserContext.UserName);
    }

    [Test]
    public async Task Load_comments()
    {
        var cmt = await _projectService.AddComment(_anchorProject!.Id,
            $"Hello from {_user1.DisplayName}");
        SwitchUser(_user2);
        var cmt1 = await _projectService.AddComment(_anchorProject.Id,
            $"Hello from {_user2.DisplayName}");

        var cmts = await _projectService.LoadComments(_anchorProject.Id);

        cmts.Should().Contain(c => c.Id == cmt.Id || c.Id == cmt1.Id);
    }

    [Test]
    public async Task Invite_user()
    {
        await _projectService.InviteUserToProject(_anchorProject!.Id, _user2.UserName);
        _anchorProject = await _projectService.GetProject(_anchorProject.Id);
        _anchorProject!.UserProjects.Should().Contain(u => u.User.Id == _user2.Id);
    }

    [Test]
    public async Task Cannot_invite_invalid_user()
    {
        Func<Task> act = async () =>
            await _projectService.InviteUserToProject(_anchorProject!.Id, "dummy_non_existance_user_name");

        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    #region Sections

    

    #endregion
}