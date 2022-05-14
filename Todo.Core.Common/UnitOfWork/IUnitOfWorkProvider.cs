namespace Todo.Core.Common.UnitOfWork;

public interface IUnitOfWorkProvider
{
    IUnitOfWork Provide();

    IStatelessUnitOfWork ProvideStateless();
}