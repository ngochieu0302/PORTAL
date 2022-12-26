﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.OpenID
{
	public partial class sys_schema_config
	{
		public decimal id { get; set; }
		public Nullable<decimal> schema_id { get; set; }
		public string envcode { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public Nullable<decimal> createdate { get; set; }
		public string createby { get; set; }
		public Nullable<decimal> updatedate { get; set; }
		public string updateby { get; set; }
		public Nullable<decimal> isactive { get; set; }
	}
}
