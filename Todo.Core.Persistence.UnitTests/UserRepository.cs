using System;
using System.Threading.Tasks;
using Autofac;
using NHibernate.Linq;
using NUnit.Framework;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTests;

[TestFixture]
public class UserRepositoryTests : BaseTest
{
    [SetUp]
    public void Setup()
    {
        _userRepository = _scope.Resolve<IUserRepository>();
    }

    private IUserRepository _userRepository;

    [Test]
    [Order(1)]
    public async Task Add_user_should_work_correctly()
    {
        var user = new User
        {
            FirstName = "Test User",
            UserId = "dummyUser" + Guid.NewGuid()
        };
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _userRepository.Add(user));
        Assert.IsTrue(user.Id > 0);
    }

    [Test]
    [Order(2)]
    public async Task Get_all_users_should_work_correctly()
    {
        await using var uow = _unitOfWorkProvider.Provide();
        var user = await _userRepository.GetAll().ToListAsync();
        Assert.IsTrue(user.Count > 0);
    }

    [Test]
    [Order(3)]
    public async Task Save_user_should_work_correctly()
    {
        var user = await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync();
            user.FirstName = "New user name" + Guid.NewGuid();
            return await _userRepository.Save(user);
        });

        var updatedUser = await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _userRepository.GetByKey(user.Id));

        Assert.AreEqual(updatedUser.FirstName, user.FirstName);
    }

    [Test]
    [Order(4)]
    public async Task Delete_user_should_work_correctly()
    {
        var user = await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync();
            await _userRepository.Delete(user);
            return user;
        });

        await using var uow1 = _unitOfWorkProvider.Provide();
        var u = await _userRepository.GetByKey(user.Id);
        Assert.IsNull(u);
    }
}