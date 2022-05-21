using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.SqlCommand;

namespace Todo.Core.Persistence.SessionFactory;

public class LoggingInterceptor : EmptyInterceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override SqlString OnPrepareStatement(SqlString sql)
    {
        _logger.LogInformation(sql.ToString());
        return sql;
    }
}