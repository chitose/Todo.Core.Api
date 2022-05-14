using NHibernate;

namespace Todo.Core.Common.UnitOfWork;

public interface IStatelessUnitOfWork : IBaseUnitOfWork<IStatelessSession>
{
}