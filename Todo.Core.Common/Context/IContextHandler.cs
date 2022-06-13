namespace Todo.Core.Common.Context;

public interface IContextHandler
{
    T? GetValue<T>(string key);

    void SetValue<T>(string key, T value);

    void CreateChildContext();
}