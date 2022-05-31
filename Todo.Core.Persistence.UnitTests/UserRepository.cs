using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.UnitTests;

[TestFixture]
public class UserRepositoryTests : BaseTest
{
    [SetUp]
    public void Setup()
    {
        _userStore = _scope.Resolve<IUserStore<User>>();
        _userManager = _scope.Resolve<UserManager<User>>();
    }

    private IUserStore<User> _userStore;
    private UserManager<User> _userManager;

    [Test]
    public async Task CreateUser()
    {
        var user = new User
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "test@gmail.com"
        };

        var result = await _userManager.CreateAsync(user, "N0P@ssw0rd4Ever");
        Assert.IsTrue(result.Succeeded);
    }
}