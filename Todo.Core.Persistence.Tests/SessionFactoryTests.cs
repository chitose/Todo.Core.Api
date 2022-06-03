using System;
using Autofac;
using FluentAssertions;
using NHibernate;
using NUnit.Framework;
using Todo.Core.Common.Tests;

namespace Todo.Core.Persistence.Tests;

[TestFixture]
public class SessionFactoryTests : BaseTest
{
    [Test]
    public void Mapping_should_work_correctly()
    {
        Action act = () => _scope.Resolve<ISessionFactory>();
        act.Should().NotThrow();
    }
}