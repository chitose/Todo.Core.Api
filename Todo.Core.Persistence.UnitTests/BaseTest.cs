using System.Threading;
using System.Threading.Tasks;
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
        UserContext.UserName = "User for test";
        UserContext.UserId = TestUserId;
        _dataCreator = new DataCreator(_scope);

        var userRepo = _scope.Resolve<IUserRepository>();
        _unitOfWorkProvider = _scope.Resolve<IUnitOfWorkProvider>();
        // create test user if not exist
        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            _user1 = await userRepo.GetByUserId(TestUserId);
            if (_user1 == null)
            {
                _user1 = new User
                {
                    UserId = TestUserId,
                    DisplayName = "User for test"
                };
                await userRepo.Add(_user1);
            }

            _user2 = await userRepo.GetByUserId(TestUserId1);
            if (_user2 == null)
            {
                _user2 = new User
                {
                    UserId = TestUserId1,
                    DisplayName = "User 1 for test"
                };
                await userRepo.Add(_user2);
            }
        });

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
        UserContext.UserId = user.UserId;
        UserContext.UserName = user.DisplayName;
    }
}