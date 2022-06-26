using System.Collections.Generic;
using NHibernate;
using NSubstitute;
using NUnit.Framework;
using Todo.Core.Common.Configuration;
using Todo.Core.Common.UnitOfWork;

namespace Todo.Core.Common.Tests;

public class UnitOfWorkTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Nested_Unit_Of_Work_Should_Work_Correctly()
    {
        var sessionFactoryMock = Substitute.For<ISessionFactory>();
        sessionFactoryMock.OpenSession().Returns(Substitute.For<ISession>());
        var uowProvider = new UnitOfWorkProvider(sessionFactoryMock,Substitute.For<IConfigProvider>() ,new List<ISessionListener>());
        using (var uow = uowProvider.Provide())
        {
            Assert.AreEqual(uow, UnitOfWork.UnitOfWork.Current);
            using (var childOow = uowProvider.Provide())
            {
                Assert.AreEqual(childOow, UnitOfWork.UnitOfWork.Current);
            }

            Assert.AreEqual(uow, UnitOfWork.UnitOfWork.Current);
        }
    }
}