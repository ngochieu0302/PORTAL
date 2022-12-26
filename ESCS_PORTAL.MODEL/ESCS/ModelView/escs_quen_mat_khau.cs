using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.ESCS_PORTAL.ModelView
{
    public class escs_quen_mat_khau<T>
    {
        public string link_lien_ket { get; set; }
        public string otp { get; set; }
        public escs_nsd nsd { get; set; }
        public escs_doi_tac doi_tac { get; set; }
        public escs_mau_email mau_email { get; set; }
        public escs_server server { get; set; }
        public T data { get; set; }
    }
    public class escs_nsd
    {
        public string ma_doi_tac { get; set; }
        public string ma_chi_nhanh { get; set; }
        public string phong { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public decimal? ngay_sinh { get; set; }
        public string dthoai { get; set; }
        public string email { get; set; }
        public decimal? ngay_hl { get; set; }
        public decimal? ngay_kt { get; set; }
        public string nsd { get; set; }
        public Nullable<DateTime> ngay { get; set; }
    }
    public class escs_doi_tac
    {
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
        public string nsd { get; set; }
        public Nullable<DateTime> ngay { get; set; }
    }
    public class escs_mau_email
    {
        public string ma_doi_tac { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string url { get; set; }
        public string url_banner { get; set; }
        public decimal? ngay_ht { get; set; }
        public decimal? ap_dung { get; set; }
    }
    public class escs_server
    {
        public string ma_doi_tac { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string smtp_server { get; set; }
        public string smtp_tai_khoan { get; set; }
        public string smtp_mat_khau { get; set; }
        public decimal? smtp_port { get; set; }
        public decimal? dung_proxy { get; set; }
        public string proxy_server { get; set; }
        public string proxy_tai_khoan { get; set; }
        public string proxy_mat_khau { get; set; }
        public decimal? ap_dung { get; set; }
        public string ten_hthi { get; set; }
    }
    public class escs_quyen_mk_token
    {
        public string token { get; set; }
    }
}
