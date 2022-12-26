using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.ESCS
{
    public class ma_doi_tac
    {
        [JsonIgnore]
        public IFormFile file_logo { get; set; }
        public string ma_doi_tac_nsd { get; set; }
        public string ma_chi_nhanh_nsd { get; set; }
        public string nsd { get; set; }
        public string pas { get; set; }

        public string ma { get; set; }
        public string ten { get; set; }
        public string ten_tat { get; set; } 
        public string ten_e { get; set; }
        public string dchi { get; set; }
        public string mst { get; set; }
        public string d_thoai { get; set; }
        public string email { get; set; }
        public string so_tk { get; set; }
        public string ngan_hang { get; set; }
        public string logo { get; set; }
        public Nullable<DateTime> ngay { get; set; }
        public string domain { get; set; }
    }
}
