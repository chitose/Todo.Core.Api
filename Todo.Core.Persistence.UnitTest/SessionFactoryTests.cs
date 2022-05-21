using Autofac;
using NHibernate;
using NUnit.Framework;

namespace Todo.Core.Persistence.UnitTest;

public class SessionFactoryTests : BaseTest
{
    [Test]
    public void Mapping_should_work_correctly()
    {
        Assert.DoesNotThrow(() =>
        {
            var session = _scope.Resolve<ISessionFactory>();
            Assert.IsNotNull(session);
        });
    }
}