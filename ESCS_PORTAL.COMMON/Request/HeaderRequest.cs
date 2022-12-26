using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Request
{
    public class HeaderRequest
    {
        public string partner_code { get; set; }
        public string token { get; set; }
        public string action { get; set; }
        public string envcode { get; set; }
        public HeaderRequest()
        {

        }
        public HeaderRequest(string partner_code, string token, string action)
        {
            this.partner_code = partner_code;
            this.token = token;
            this.action = action;
        }
    }
}
