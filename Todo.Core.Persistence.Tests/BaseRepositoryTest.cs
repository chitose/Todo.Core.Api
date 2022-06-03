using Autofac;
using Todo.Core.Common.Tests;

namespace Todo.Core.Persistence.Tests;

public abstract class BaseRepositoryTest : BaseTest
{
    protected DataCreator _dataCreator;

    public new void OneTimeSetup()
    {
        _dataCreator = _scope.Resolve<DataCreator>();
    }
}