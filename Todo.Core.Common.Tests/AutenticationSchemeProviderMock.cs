using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Todo.Core.Common.Tests;

public class AutenticationSchemeProviderMock : AuthenticationSchemeProvider
{
    public AutenticationSchemeProviderMock(IOptions<AuthenticationOptions> options) : base(options)
    {
    }

    protected AutenticationSchemeProviderMock(IOptions<AuthenticationOptions> options,
        IDictionary<string, AuthenticationScheme> schemes) : base(options, schemes)
    {
    }
}