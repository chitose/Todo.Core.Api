using Autofac;
using Microsoft.AspNetCore.Builder;
using NUnit.Framework;
using Todo.Core.Common.Autofac;

namespace Todo.Core.Persistence.UnitTest;

public abstract class BaseTest
{
    protected ILifetimeScope _scope;
    [SetUp]
    public void Setup()
    {
        var builder = WebApplication.CreateBuilder();
        var container = builder.UseAutofac(new[] { "Todo.Core.*.dll" });
        _scope = container.BeginLifetimeScope();
    }

    [TearDown]
    public void Teardown()
    {
        _scope.Dispose();
    }
}