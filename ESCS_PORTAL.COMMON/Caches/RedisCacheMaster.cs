using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Caches
{
    public class RedisCacheMaster
    {
        public static string ConnectionName { get; set; }
        public static string Endpoint { get; set; }
        public static int DatabaseIndex { get; set; }
    }
}
