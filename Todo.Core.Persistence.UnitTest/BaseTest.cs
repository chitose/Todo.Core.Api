using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using NUnit.Framework;
using Todo.Core.Common.Autofac;
using Todo.Core.Common.Context;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Common.UnitTests;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTest;

public abstract class BaseTest
{
    private readonly string? TestUserId = "5B05D0F7-C9CA-4315-A1C2-C9CA4ADCD28A";
    protected DataCreator _dataCreator;
    protected ILifetimeScope _scope;
    protected IUnitOfWorkProvider _unitOfWorkProvider;
    private ExecutionContext _executionCtx;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        var builder = WebApplication.CreateBuilder();
        var container = builder.UseAutofac(new[] { "Todo.Core.*.dll" });
        _scope = container.BeginLifetimeScope();
        UserContext.UserName = "User for test";
        UserContext.UserId = TestUserId;
        _dataCreator = new DataCreator(_scope);

        var userRepo = _scope.Resolve<IUserRepository>();
        _unitOfWorkProvider = _scope.Resolve<IUnitOfWorkProvider>();
        // create test user if not exist
        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var user = await userRepo.GetByUserId(TestUserId);
            if (user == null)
            {
                user = new User
                {
                    UserId = TestUserId,
                    DisplayName = "User for test"
                };
                user = await userRepo.Add(user);
            }

            return user;
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
}