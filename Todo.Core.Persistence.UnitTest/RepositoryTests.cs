using System.Threading.Tasks;
using Autofac;
using NHibernate.Linq;
using NUnit.Framework;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTest;

public class RepositoryTests : BaseTest
{
    private IUnitOfWorkProvider _unitOfWorkProvider;
    private IUserRepository _userRepository;
    [SetUp]
    public new void Setup()
    {
        _unitOfWorkProvider = _scope.Resolve<IUnitOfWorkProvider>();
        _userRepository = _scope.Resolve<IUserRepository>();
    }
    [Test]
    [Order(1)]
    public async Task Repository_Add_Should_work()
    {
        var user = new User
        {
            FirstName = "Test User",
            UserId = "dummyUser"
        };
        using var uow = _unitOfWorkProvider.Provide();
        uow.GetCurrentSession().Save(user);
        uow.Commit();
        //await _userRepository.Add(user);
        //uow.GetCurrentSession().Flush();
        //await uow.CommitAsync();
        Assert.IsTrue(user.Id > 0);
    }

    [Test]
    [Order(2)]
    public async Task Reopistory_GetAll_Should_Work()
    {
        using var uow = _unitOfWorkProvider.Provide();
        var user = await _userRepository.GetAll().ToListAsync();
        Assert.IsTrue(user.Count > 0);
    }
}