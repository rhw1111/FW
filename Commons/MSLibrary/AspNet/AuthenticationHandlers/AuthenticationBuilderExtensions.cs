using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using MSLibrary.AspNet.AuthenticationHandlers;

namespace MSLibrary.AspNet.AuthenticationHandlers
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddTokenScheme(this AuthenticationBuilder builder, string schema,Action<TokenAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(schema,configureOptions);
        }
    }
}
