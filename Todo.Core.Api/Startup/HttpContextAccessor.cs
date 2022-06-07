using Todo.Core.Common.Startup;

namespace Todo.Core.Api.Startup;

public class HttpContextAccessorStartupConfiguration : IBuilderStartupConfiguration
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
    }
}