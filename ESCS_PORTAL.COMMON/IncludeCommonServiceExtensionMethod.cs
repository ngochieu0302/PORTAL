using ESCS_PORTAL.COMMON.Caches;
using ESCS_PORTAL.COMMON.Caches.interfaces;
using ESCS_PORTAL.COMMON.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON
{
    public static class IncludeCommonServiceExtensionMethod
    {
        public static void AddCommonService(this IServiceCollection services)
        {
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddScoped(typeof(ILogRequestRepository<>), typeof(LogRequestRepository<>));
            services.AddScoped(typeof(ILogRequestService<>), typeof(LogRequestService<>));
            services.AddScoped<ICacheServer, CacheServer>();
            services.AddScoped<IMemoryCacheManager, MemoryCacheManager>();
        }
    }
}
