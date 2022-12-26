using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Common
{
    public class Document
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}
