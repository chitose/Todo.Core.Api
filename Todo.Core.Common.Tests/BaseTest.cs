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

namespace Todo.Core.Common.Tests;

public abstract class BaseTest
{
    protected const string TestUserPassword = "N0P@ssw0rd4Ever";
    protected readonly string? TestUsername = "5B05D0F7-C9CA-4315-A1C2-C9CA4ADCD28A";
    protected readonly string? TestUsername2 = "BDAC1CBA-32FA-49B8-8512-A328E8083687";
    protected IContainer _container;

    private ExecutionContext _executionCtx;
    protected ILifetimeScope _scope;
    protected IUnitOfWorkProvider _unitOfWorkProvider;

    protected User _user1;
    protected User _user2;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        var builder = WebApplication.CreateBuilder();
        _container = builder.UseAutofac(new[]
        {
            "Todo.Core.*.dll"
        });
        using var scope = _container.BeginLifetimeScope();
        _unitOfWorkProvider = scope.Resolve<IUnitOfWorkProvider>();
        var userRepo = scope.Resolve<IUserRepository>();

        _user1 = await userRepo.FindByUserName(TestUsername);
        if (_user1 == null)
            _user1 = await userRepo.CreateUser(new User
            {
                UserName = TestUsername,
                DisplayName = "User 1",
                Email = "test@todo.com"
            }, TestUserPassword);

        _user2 = await userRepo.FindByUserName(TestUsername2);
        if (_user2 == null)
            _user2 = await userRepo.CreateUser(new User
            {
                UserName = TestUsername2,
                DisplayName = "User 2",
                Email = "test1@todo.com"
            }, TestUserPassword);

        UserContext.UserName = TestUsername;
        UserContext.UserDisplayName = _user1.DisplayName;

        SaveExecutionContext();
    }

    [SetUp]
    public void Setup()
    {
        RestoreExecutionContext();
        _scope = _container.BeginLifetimeScope();
    }

    [TearDown]
    public void Teardown()
    {
        _scope.Dispose();
    }

    protected void SaveExecutionContext()
    {
        _executionCtx = ExecutionContext.Capture();
    }

    protected void RestoreExecutionContext()
    {
        ExecutionContext.Restore(_executionCtx);
    }

    protected Task RunWithContextOfUser(User user, Func<Task> action)
    {
        return Task.Run(async () =>
        {
            var content = UserContext.GetContent();
            try
            {
                UserContext.CreateChildContext();
                UserContext.UserName = user.UserName;
                UserContext.UserDisplayName = user.DisplayName;
                await action();
            }
            finally
            {
                UserContext.RestoreFromContent(content);
            }
        });
    }

    protected Task<T> RunWithContextOfUser<T>(User user, Func<Task<T>> action)
    {
        return Task.Run(async () =>
        {
            var content = UserContext.GetContent();
            try
            {
                UserContext.CreateChildContext();
                UserContext.UserName = user.UserName;
                UserContext.UserDisplayName = user.DisplayName;
                return await action();
            }
            finally
            {
                UserContext.RestoreFromContent(content);
            }
        });
    }
}