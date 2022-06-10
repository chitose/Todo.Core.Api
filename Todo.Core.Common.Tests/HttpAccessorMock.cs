using Microsoft.AspNetCore.Http;

namespace Todo.Core.Common.Tests;

public class HttpAccessorMock : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; } = new DefaultHttpContext();
}