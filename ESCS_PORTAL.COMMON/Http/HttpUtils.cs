using ESCS_PORTAL.COMMON.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Http
{
    public class HttpUtils
    {
        public static string GetJsonBaseRequest(string json_data_info, DefineInfo defineInfo)
        {
            if (string.IsNullOrEmpty(json_data_info))
            {
                json_data_info = "{}";
            }
            var sb = new StringBuilder();
            sb.Append("{\"define_info\":{\"accept\":\"" + defineInfo.accept + "\",\"accept_encoding\":\"" + defineInfo.accept_encoding + "\",\"host\":\"" + defineInfo.host + "\",\"referer\":\"" + defineInfo.referer + "\",\"user_agent\":\"" + defineInfo.user_agent + "\",\"origin\":\"" + defineInfo.origin + "\",\"ip_remote_ipv4\":\"" + defineInfo.ip_remote_ipv4 + "\",\"ip_remote_ipv6\":\"" + defineInfo.ip_remote_ipv6 + "\",\"time\":" + defineInfo.time + " },\"data_info\":" + json_data_info + "}");
            return sb.ToString();
        }
    }
}
