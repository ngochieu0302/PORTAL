using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Common
{
    public class OpenIDConfig
    {
        public static string ConnectString { get; set; }
        public static string DbName { get; set; }
        public static string Schema { get; set; }
        public static string RedisOrMemoryCache { get; set; }
        public static int TimeLiveAccessTokenMinute { get; set; }
        public static int TimeCantUseRefreshToken { get; set; }
        public static int TimeLiveDataCacheMinute { get; set; }
    }
}
