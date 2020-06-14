using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace FW.TestPlatform.Main.AspNet.AuthenticationHandlers
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddDefaultScheme(this AuthenticationBuilder builder, string schema, Action<DefaultOptions> configureOptions)
        {
            return builder.AddScheme<DefaultOptions, DefaultAuthenticationHandler>(schema, configureOptions);
        }
    }
}
