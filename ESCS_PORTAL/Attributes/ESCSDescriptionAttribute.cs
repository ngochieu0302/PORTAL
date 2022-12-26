using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCS_PORTAL.Attributes
{
    public class ESCSMethod
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string DELETE = "DELETE";
    }
    public class ESCSDescriptionAttribute : Attribute
    {
        public string Method { get; set; }
        public string Description { get; set; }
        public ESCSDescriptionAttribute(string Method, string Description)
        {
            this.Method = Method;
            this.Description = Description;
        }
    }
}
