using System.Threading;
using System.Threading.Tasks;
using Antlr.Runtime.Misc;
using Autofac;
using Microsoft.AspNetCore.Builder;
using NUnit.Framework;
using Todo.Core.Common.Autofac;
using Todo.Core.Common.Context;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTests;

public abstract class BaseTest
{
    protected const string TestUserPassword = "N0P@ssw0rd4Ever";
    protected readonly string? TestUserId = "5B05D0F7-C9CA-4315-A1C2-C9CA4ADCD28A";
    protected readonly string? TestUserId1 = "BDAC1CBA-32FA-49B8-8512-A328E8083687";

    protected DataCreator _dataCreator;
    private ExecutionContext _executionCtx;
    protected ILifetimeScope _scope;
    protected IUnitOfWorkProvider _unitOfWorkProvider;

    protected User _user1;
    protected User _user2;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        var builder = WebApplication.CreateBuilder();
        var container = builder.UseAutofac(new[]
        {
            "Todo.Core.*.dll"
        });
        _scope = container.BeginLifetimeScope();
        UserContext.UserDisplayName = "User for test";
        UserContext.UserId = TestUserId;
        _dataCreator = new DataCreator(_scope);
        _unitOfWorkProvider = _scope.Resolve<IUnitOfWorkProvider>();
        var userRepo = _scope.Resolve<IUserRepository>();

        var user1 = await userRepo.FindByUserName(TestUserId);
        if (user1 == null)
        {
            await userRepo.CreateUser(new User
            {
                UserName = TestUserId,
                Email = "test@todo.com"
            }, TestUserPassword);
        }

        var user2 = await userRepo.FindByUserName(TestUserId1);
        if (user2 == null)
        {
            await userRepo.CreateUser(new User
            {
                UserName = TestUserId1,
                Email = "test1@todo.com"
            }, TestUserPassword);
        }

        SaveExecutionContext();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        RestoreExecutionContext();
        await _scope.DisposeAsync();
    }

    [SetUp]
    public void Setup()
    {
        RestoreExecutionContext();
    }

    protected void SaveExecutionContext()
    {
        _executionCtx = ExecutionContext.Capture();
    }

    protected void RestoreExecutionContext()
    {
        ExecutionContext.Restore(_executionCtx);
    }

    protected void SwitchUser(User user)
    {
        UserContext.UserId = user.UserName;
        UserContext.UserDisplayName = user.DisplayName;
    }

    protected void RunWithContextOfUser(User user, Action action)
    {
        var oldUserName = UserContext.UserId;
        var oldUserDispName = UserContext.UserDisplayName;
        UserContext.UserId = user.UserName;
        UserContext.UserDisplayName = user.DisplayName;
        try
        {
            action();
        }
        finally
        {
            UserContext.UserId = oldUserName;
            UserContext.UserDisplayName = oldUserDispName;
        }
    }
}