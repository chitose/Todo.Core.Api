namespace Todo.Core.Common.UnitOfWork;

public interface ISessionListener
{
    void OnOpen(ISessionAccessor accessor);
    
    string ConnectionType { get; }
}