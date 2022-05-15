using System;
using System.Threading.Tasks;
using Autofac;
using NHibernate.Linq;
using NUnit.Framework;
using Todo.Core.Common.Context;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTest;

public class UserRepositoryTests : BaseTest
{
    private IUnitOfWorkProvider _unitOfWorkProvider;
    private IUserRepository _userRepository;
    
    [SetUp]
    public new void Setup()
    {
        _unitOfWorkProvider = _scope.Resolve<IUnitOfWorkProvider>();
        _userRepository = _scope.Resolve<IUserRepository>();
        UserContext.UserName = "Test";
    }
    [Test]
    [Order(1)]
    public async Task Add_User_Should_Work_Correctly()
    {
        var user = new User
        {
            FirstName = "Test User",
            UserId = "dummyUser" + Guid.NewGuid()
        };
        await using var uow = _unitOfWorkProvider.Provide();
        await _userRepository.Add(user);
        await uow.CommitAsync();
        Assert.IsTrue(user.Id > 0);
    }

    [Test]
    [Order(2)]
    public async Task Get_All_Users_Should_Work_Correctly()
    {
        await using var uow = _unitOfWorkProvider.Provide();
        var user = await _userRepository.GetAll().ToListAsync();
        Assert.IsTrue(user.Count > 0);
    }

    [Test]
    [Order(3)]
    public async Task Save_User_Should_Work_Correctly()
    {
        await using var uow = _unitOfWorkProvider.Provide();
        var user = await _userRepository.GetAll().FirstOrDefaultAsync();
        user.FirstName = "New user name" + Guid.NewGuid();
        await _userRepository.Save(user);
        await uow.CommitAsync();

        await using var uow1 = _unitOfWorkProvider.Provide();
        var updatedUser = await _userRepository.GetByKey(user.Id);
        
        Assert.AreEqual(updatedUser.FirstName, user.FirstName);
    }

    [Test]
    [Order(4)]
    public async Task Delete_User_Should_Work_Correctly()
    {
        await using var uow = _unitOfWorkProvider.Provide();
        var user = await _userRepository.GetAll().FirstOrDefaultAsync();
        await _userRepository.Delete(user);
        await uow.CommitAsync();

        await using var uow1 = _unitOfWorkProvider.Provide();
        var u = await _userRepository.GetByKey(user.Id);
        Assert.IsNull(u);
    }
}