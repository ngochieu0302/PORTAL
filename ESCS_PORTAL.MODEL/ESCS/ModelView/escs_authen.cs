using ESCS_PORTAL.COMMON.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.ESCS_PORTAL.ModelView
{
    public class escs_authen
    {
        public escs_nguoi_dung nguoi_dung { get; set; }
        public IEnumerable<escs_phan_quyen> phan_quyen { get; set; }
        public IEnumerable<escs_dvi_phan_quyen> dvi_phan_quyen { get; set; }
        public IEnumerable<escs_menu> menu { get; set; }
        public escs_dv_google dv_google { get; set; }
        public long? time_live { get; set; }
    }
    public class escs_phan_quyen
    {
        public string ma { get; set; }
        public string nhom_chuc_nang { get; set; }
        public string nhap { get; set; }
        public string xem { get; set; }
    }
    public class escs_dvi_phan_quyen
    {
        public string ma_doi_tac_ql { get; set; }
        public string ma_chi_nhanh_ql { get; set; }
    }
    public class escs_menu
    {
        public string ma_doi_tac { get; set; }
        public Nullable<decimal> so_id { get; set; }
        public string ten { get; set; }
        public Nullable<decimal> so_id_cha { get; set; }
        public Nullable<decimal> stt { get; set; }
        public string url { get; set; }
        public string nhom_quyen { get; set; }
        public string icon { get; set; }
        public string target { get; set; }
        public Nullable<decimal> hien_thi { get; set; }
        public string nhom { get; set; }
    }
    public class escs_dv_google
    {
        public string ma_doi_tac { get; set; }
        public string ma_chi_nhanh { get; set; }
        public string key { get; set; }
        public string ngay_ht { get; set; }
        public string nsd { get; set; }
        public string ap_dung { get; set; }
        public decimal? ngay_hl { get; set; }
        public decimal? ngat_kt { get; set; }
    }
    public class nsd_y_kien 
    {
        public string ma_doi_tac { get; set; }
        public Nullable<long> so_id_hs { get; set; }
        public Nullable<long> so_id_yk { get; set; }
        public string nhom { get; set; }
        public string hash_code { get; set; }
        public string nsd { get; set; }
    }
}
