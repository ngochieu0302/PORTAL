using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ESCS_PORTAL.MODEL.ESCS
{
    public class EsMailInfo
    {
        public string ma_doi_tac_nsd { get; set; }
        public string ma_chi_nhanh_nsd { get; set; }
        public string nsd { get; set; }
        public string pas { get; set; }

        public string key { get; set; }
        public string ma_doi_tac { get; set; }
        public string so_id { get; set; }
        public string loai { get; set; }
        public string nguoi_nhan { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung { get; set; }
        public string template { get; set; }
        //public List<IFormFile> files { get; set; }
        public List<EsMailInfoFile> files { get; set; }
    }
    public class EsMailInfoFile
    {
        public string file_base_64 { get; set; }
        public string ten_file { get; set; }
    }

}
