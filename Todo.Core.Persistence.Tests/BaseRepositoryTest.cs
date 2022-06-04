using Autofac;
using NUnit.Framework;
using Todo.Core.Common.Tests;

namespace Todo.Core.Persistence.Tests;

public abstract class BaseRepositoryTest : BaseTest
{
    protected DataCreator _dataCreator;

    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _dataCreator = new DataCreator(_scope);
    }
}