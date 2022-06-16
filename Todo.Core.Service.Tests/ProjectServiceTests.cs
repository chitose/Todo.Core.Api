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
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Exceptions;
using Todo.Core.Persistence.Repositories;
using Todo.Core.Service.Project;

namespace Todo.Core.Service.Tests;

[TestFixture]
public class ProjectServiceTests : BaseTest
{
    private Persistence.Entities.Project? _anchorProject;

    private IProjectService _projectService = null!;

    private ITaskRepository _taskRepository = null!;

    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        RestoreExecutionContext();
        _projectService = _scope.Resolve<IProjectService>();
        _taskRepository = _scope.Resolve<ITaskRepository>();
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
        _anchorProject = await _projectService.GetProject(_anchorProject!.Id);
    }

    #region Project CRUD

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
        Assert.IsTrue(prj.Order > _anchorProject!.Order);
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
            AboveProject = _anchorProject!.Id
        });

        Assert.IsTrue(prj.Order < _anchorProject.Order);

        var belowPrj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "below project",
            BelowProject = _anchorProject.Id
        });

        Assert.IsTrue(belowPrj.Order > _anchorProject.Order);

        var act = async () => await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "Test",
            BelowProject = 1,
            AboveProject = 2
        });

        await act.Should().ThrowAsync<TodoException>();
    }

    [Test]
    public async Task Update_project()
    {
        var act = async () => await _projectService.UpdateProject(-1, new ProjectCreationInfo());

        await act.Should().ThrowAsync<ProjectNotFoundException>();

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
    public async Task Cannot_update_archived_project()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo()
        {
            Name = "Archived project",
            Archived = true
        });

        Func<Task> act = async () => await _projectService.UpdateProject(prj.Id, new ProjectCreationInfo
        {
            Name = "new name"
        });

        await act.Should().ThrowAsync<TodoException>().WithMessage($"Cannot modify archived project");
    }

    [Test]
    public async Task Collaborator_can_update_project()
    {
        var user1Project = await RunWithContextOfUser(_user1, async () =>
        {
            var prj = await _projectService.CreateProject(new ProjectCreationInfo
            {
                Name = "Project"
            });

            await _projectService.InviteUserToProject(prj.Id, _user2.UserName);

            return prj;
        });

        await RunWithContextOfUser(_user2, async () =>
        {
            const string updateProjectName = "Update from user 2";
            // ReSharper disable once AccessToModifiedClosure
            Func<Task> act = async () => await _projectService.UpdateProject(user1Project.Id, new ProjectUpdateInfo
            {
                Name = updateProjectName
            });

            await act.Should().NotThrowAsync<ProjectNotFoundException>();
            user1Project = await _projectService.GetProject(user1Project.Id);
            user1Project!.Name.Should().BeEquivalentTo(updateProjectName);
        });
    }

    [Test]
    public async Task Only_project_collaborator_can_see_project()
    {
        var prj = await RunWithContextOfUser(_user1, async () => await _projectService.CreateProject(
            new ProjectCreationInfo
            {
                Name = "test"
            }));

        await RunWithContextOfUser(_user2, async () =>
        {
            Func<Task<Persistence.Entities.Project?>> act = async () => await _projectService.GetProject(prj.Id);
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
        var prj = await RunWithContextOfUser(_user1, async () =>
        {
            var prj = await _projectService.CreateProject(
                new ProjectCreationInfo
                {
                    Name = "User 1 project"
                });
            prj.AuthorId.Should().Be(UserContext.UserName);

            await _projectService.InviteUserToProject(prj.Id, _user2.UserName);
            return prj;
        });

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

    #endregion

    [Test]
    public async Task Remove_project_collaborator()
    {
        Func<Task> act = async () => await _projectService.RemoveUserFromProject(-1, _user2.UserName);

        await act.Should().ThrowAsync<ProjectNotFoundException>();

        await RunWithContextOfUser(_user1, async () =>
        {
            var prj = await _projectService.CreateProject(new ProjectCreationInfo
            {
                Name = "User 1 project"
            });

            // ReSharper disable once AccessToModifiedClosure
            act = async () => await _projectService.RemoveUserFromProject(prj.Id, _user2.UserName);

            await act.Should().ThrowAsync<TodoException>()
                .WithMessage("Cannot leave/remove user from project without any collaborator.");

            await _projectService.InviteUserToProject(prj.Id, _user2.UserName);

            prj = await _projectService.GetProject(prj.Id);

            prj!.UserProjects.Count.Should().Be(2);

            await _projectService.RemoveUserFromProject(prj.Id, _user2.UserName);

            prj = await _projectService.GetProject(prj.Id);

            prj!.UserProjects.Count.Should().Be(1);
        });
    }

    [Test]
    public async Task Leave_project()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "User 1 project"
        });

        await _projectService.InviteUserToProject(prj.Id, _user2.UserName);

        prj = await _projectService.GetProject(prj.Id);

        prj!.UserProjects.Should().Contain(x => x.User.UserName == _user1.UserName && x.Owner);
        prj.UserProjects.Count.Should().Be(2);

        await _projectService.LeaveProject(prj.Id);

        await RunWithContextOfUser(_user2, async () =>
        {
            prj = await _projectService.GetProject(prj.Id);

            prj!.UserProjects.Count.Should().Be(1);
            prj.UserProjects.Should().Contain(x => x.User.UserName == _user2.UserName && x.Owner);
        });
    }

    [Test]
    public async Task Leave_not_collaborated_project_or_none_existence()
    {
        Func<Task> act = async () => await _projectService.LeaveProject(-1);

        await act.Should().ThrowAsync<ProjectNotFoundException>();

        var project = await RunWithContextOfUser(_user1, async () =>
        {
            var prj = _projectService.CreateProject(new ProjectCreationInfo() {Name = "User 1 project"});
            Func<Task> act1 = async () => await _projectService.LeaveProject(prj.Id);

            await act1.Should().ThrowAsync<TodoException>()
                .WithMessage("Cannot leave/remove user from project without any collaborator.");

            return prj;
        });

        act = async () =>
            await RunWithContextOfUser(_user2, () => _projectService.LeaveProject(project.Id));

        await act.Should().ThrowAsync<ProjectNotFoundException>();
    }

    [Test]
    public async Task Swap_projects_order()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "above anchor",
            AboveProject = _anchorProject!.Id
        });

        var prj1 = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "below anchor",
            BelowProject = _anchorProject.Id
        });

        await _projectService.SwapProjectOrder(prj.Id, prj1.Id);

        var updatePrj = await _projectService.GetProject(prj.Id);
        var updateRj1 = await _projectService.GetProject(prj1.Id);
        Assert.AreEqual(prj.Order, updateRj1!.Order);
        Assert.AreEqual(updatePrj!.Order, prj1.Order);
    }

    [Test]
    public async Task Invite_user()
    {
        Func<Task> act = async () => await _projectService.InviteUserToProject(-1, _user2.UserName);
        await act.Should().ThrowAsync<ProjectNotFoundException>();

        act = async () =>
            await _projectService.InviteUserToProject(_anchorProject!.Id, "dummy_invalid_user");

        await act.Should().ThrowAsync<UserNotFoundException>();

        await _projectService.InviteUserToProject(_anchorProject!.Id, _user2.UserName);
        _anchorProject = await _projectService.GetProject(_anchorProject.Id);
        _anchorProject!.UserProjects.Should().Contain(u => u.User.Id == _user2.Id);
    }

    [Test]
    public async Task Archive_project()
    {
        var act = async () => await _projectService.ArchiveProject(-1);

        await act.Should().ThrowAsync<ProjectNotFoundException>();

        var project = await _projectService.CreateProject(new ProjectCreationInfo() {Name = "Test"});

        var task = await _unitOfWorkProvider.PerformActionInUnitOfWorkStateless(() => _taskRepository.Add(new Persistence.Entities.TodoTask
        {
            Project = project,
            Title = "Test task"
        }));

        await _projectService.ArchiveProject(project.Id);

        project = await _projectService.GetProject(project.Id);

        project!.Archived.Should().BeTrue();

        task = await _unitOfWorkProvider.PerformActionInUnitOfWorkStateless(() => _taskRepository.GetByKey(task.Id));

        task!.Completed.Should().BeFalse();
    }

    [Test]
    public async Task Unarchive_project()
    {
        var act = async () => await _projectService.UnarchiveProject(-1);
        await act.Should().ThrowAsync<ProjectNotFoundException>();

        var project = await _projectService.CreateProject(new ProjectCreationInfo() {Name = "Test"});

        await _projectService.ArchiveProject(project.Id);

        await _projectService.UnarchiveProject(project.Id);

        project = await _projectService.GetProject(project.Id);

        project!.Archived.Should().BeFalse();
    }

    [Test]
    public async Task Add_comment()
    {
        Func<Task> act = async () => await _projectService.AddComment(-1, "Hello");

        await act.Should().ThrowAsync<ProjectNotFoundException>();

        var cmt = await _projectService.AddComment(_anchorProject!.Id,
            "Hello");

        cmt.Should().NotBeNull();
        cmt.Id.Should().BePositive();
        cmt.AuthorId.Should().Be(UserContext.UserName);
    }

    [Test]
    public async Task Load_comments()
    {
        var cmt = await RunWithContextOfUser(_user1, async () => await _projectService.AddComment(_anchorProject!.Id,
            $"Hello from {_user1.DisplayName}"));


        var cmt1 = await RunWithContextOfUser(_user2, async () => await _projectService.AddComment(_anchorProject!.Id,
            $"Hello from {_user2.DisplayName}"));

        var comments = await _projectService.LoadComments(_anchorProject!.Id);

        comments.Should().Contain(c => c.Id == cmt.Id || c.Id == cmt1.Id);
    }

    #region Sections

    [Test]
    public async Task Add_project_section()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo {Name = "Test project"});

        var sect = await _projectService.AddSection(prj.Id, "Test section");

        sect.Should().NotBeNull();
        sect.Id.Should().BePositive();
    }

    [Test]
    public async Task Project_collaborate_add_section()
    {
        var prj = await _projectService.CreateProject(new ProjectCreationInfo
        {
            Name = "Test project"
        });

        await _projectService.InviteUserToProject(prj.Id, _user2.UserName);

        await RunWithContextOfUser(_user2, async () =>
        {
            var sect = await _projectService.AddSection(prj.Id, "Test section");
            sect.Id.Should().BePositive();
        });
    }

    [Test]
    public async Task None_project_collaborator_cannot_add_section()
    {
        var prj = await RunWithContextOfUser(_user1,
            async () => await _projectService.CreateProject(new ProjectCreationInfo {Name = "Test project"}));

        await RunWithContextOfUser(_user2, async () =>
        {
            Func<Task<ProjectSection>> act = async () =>
                await _projectService.AddSection(prj.Id, "Test section");


            await act.Should().ThrowAsync<ProjectNotFoundException>();
        });
    }

    [Test]
    public async Task Update_project_section()
    {
        var act = async () => await _projectService.UpdateSection(-1, "Test");

        await act.Should().ThrowAsync<SectionNotFoundException>();

        var prj = await _projectService.CreateProject(new ProjectCreationInfo {Name = "Test project"});

        var sect = await _projectService.AddSection(prj.Id, "Test section");

        var updatedSect = await _projectService.UpdateSection(sect.Id, "New section name");

        sect = await _projectService.GetSection(sect.Id);

        sect!.Title.Should().Be(updatedSect.Title);
    }

    [Test]
    public async Task Collaborator_update_project_section()
    {
        var sect = await RunWithContextOfUser(_user1, async () =>
        {
            var project = await _projectService.CreateProject(new ProjectCreationInfo {Name = "Test project"});
            var section = await _projectService.AddSection(project.Id, "Test section");

            await _projectService.InviteUserToProject(project.Id, _user2.UserName);
            return section;
        });

        await RunWithContextOfUser(_user2,
            async () => { await _projectService.UpdateSection(sect.Id, "New section name"); });

        sect = await _projectService.GetSection(sect.Id);

        sect!.Title.Should().Be("New section name");
    }

    [Test]
    public async Task Load_and_swap_section_order()
    {
        var project = await _projectService.CreateProject(new ProjectCreationInfo() {Name = "Test project"});
        var section1 = await _projectService.AddSection(project.Id, "Section 1");
        var section2 = await _projectService.AddSection(project.Id, "Section 2");

        section1.Order.Should().BeLessThan(section2.Order);

        await _projectService.SwapSectionOrder(section1.Id, section2.Id);

        var sections = await _projectService.LoadSections(project.Id);

        sections.Count.Should().Be(2);

        sections.Should().BeInAscendingOrder(x => x.Order);
        sections.First().Id.Should().Be(section2.Id);
        sections.Last().Id.Should().Be(section1.Id);
    }

    [Test]
    public async Task Archive_unarchive_section_with_tasks()
    {
        var act = async () => await _projectService.ArchiveSection(-1);

        await act.Should().ThrowAsync<SectionNotFoundException>();

        var project = await _projectService.CreateProject(new ProjectCreationInfo {Name = "Test"});
        var section = await _projectService.AddSection(project.Id, "Test section");
        var task = await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
            _taskRepository.Add(new Persistence.Entities.TodoTask
            {
                Title = "Test task",
                Description = "Task for testing",
                Priority = TaskPriority.Low,
                Project = project,
                Section = section
            }));

        // archive section

        await _projectService.ArchiveSection(section.Id);

        section = await _projectService.GetSection(section.Id);

        section!.Archived.Should().BeTrue();

        task = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _taskRepository.GetByKey(task.Id));

        task!.Completed.Should().BeTrue();

        // unarchive the section

        act = async () => await _projectService.UnarchiveSection(-1);

        await act.Should().ThrowAsync<SectionNotFoundException>();

        await _projectService.UnarchiveSection(section.Id);

        section = await _projectService.GetSection(section.Id);

        section!.Archived.Should().BeFalse();

        task = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _taskRepository.GetByKey(task.Id));

        task!.Completed.Should().BeTrue();
    }

    [Test]
    public async Task Delete_section()
    {
        var act = async () => await _projectService.DeleteSection(-1);

        await act.Should().ThrowAsync<SectionNotFoundException>();

        var project = await _projectService.CreateProject(new ProjectCreationInfo() {Name = "Test"});

        var section = await _projectService.AddSection(project.Id, "Test section");

        await _projectService.DeleteSection(section.Id);

        section = await _projectService.GetSection(section.Id);

        section.Should().BeNull();
    }

    #endregion
}