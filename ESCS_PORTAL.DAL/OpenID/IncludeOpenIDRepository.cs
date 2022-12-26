﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.DAL.OpenID
{
    public static class IncludeOpenIDRepository
    {
        public static void AddOpenIDRepository(this IServiceCollection services)
        {
            services.AddScoped<IOpenIDCommonRepository, OpenIDCommonRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IErrorCodeRepository, ErrorCodeRepository>();
        }
    }
}
