using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ESCS_PORTAL.MODEL.ESCS
{
    public class ht_mau_in_upload
    {
        [JsonIgnore]
        public IFormFile file { get; set; }
        public string ma_doi_tac_nsd { get; set; }
        public string ma_chi_nhanh_nsd { get; set; }
        public string nsd { get; set; }
        public string pas { get; set; }

        public string ma_doi_tac { get; set; }
        public string ma_doi_tac_ql { get; set; }
        public decimal? ngay_ht { get; set; }
        public string ten { get; set; }
        public string ma { get; set; }
        public string url_file { get; set; }
        public string ma_action_api { get; set; }
        public string loai { get; set; }
        public string nv { get; set; }
        public decimal? trang_thai { get; set; }
        public string pm { get; set; }
        public string hien_thi_app { get; set; }
    }
}
