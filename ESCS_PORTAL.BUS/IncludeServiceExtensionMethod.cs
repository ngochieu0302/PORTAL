using ESCS_PORTAL.BUS.OpenID;
using ESCS_PORTAL.BUS.Services;
using ESCS_PORTAL.DAL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESCS_PORTAL.BUS
{
    public static class IncludeServiceExtensionMethod
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddCustomRepository();
            services.AddScoped<IDynamicService, DynamicService>();
            services.AddScoped<IOpenIdService, OpenIdService>();
            services.AddScoped<IErrorCodeService, ErrorCodeService>();
        }
    }
}
