using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.ESCS
{
    public class nguoi_su_dung
    {
        [JsonIgnore]
        public IFormFile file_anh_dai_dien { get; set; }
        public string ma_doi_tac_nsd { get; set; }
        public string ma_chi_nhanh_nsd { get; set; }
        public string nsd { get; set; }
        public string pas { get; set; }

        public string ma { get; set; }
        public string ma_doi_tac { get; set; }
        public string ma_chi_nhanh { get; set; }
        public string phong { get; set; }
        public string mat_khau { get; set; }
        public string ten { get; set; }
        public int ngay_sinh { get; set; }
        public string anh_dai_dien { get; set; }
        public string dthoai { get; set; }
        public string email { get; set; }
        public int ngay_hl { get; set; }
        public int ngay_kt { get; set; }
        public int loai_tk { get; set; }
        public int trang_thai { get; set; }
        public DateTime ngay { get; set; }
    }
}