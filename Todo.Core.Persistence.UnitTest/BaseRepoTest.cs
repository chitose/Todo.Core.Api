using Autofac;
using NUnit.Framework;
using Todo.Core.Common.Context;
using Todo.Core.Common.UnitOfWork;

namespace Todo.Core.Persistence.UnitTest;

public abstract class BaseRepoTest : BaseTest
{
    protected IUnitOfWorkProvider _unitOfWorkProvider;
    
    [SetUp]
    public new void Setup()
    {
        _unitOfWorkProvider = _scope.Resolve<IUnitOfWorkProvider>();
        UserContext.UserName = "Test";
    }
}