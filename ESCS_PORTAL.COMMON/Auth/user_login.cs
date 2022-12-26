using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESCS_PORTAL.COMMON.Auth
{
    public class user_login
    {
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        public string username { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string password { get; set; }
       
    }
    public class user_login_escs
    {
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        public string username { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string captcha { get; set; }
        public string loai_kh { get; set; }
    }
    public class user_login_escs_change_pass
    {
        public string ma_doi_tac_nsd { get; set; }
        public string ma_chi_nhanh_nsd { get; set; }
        public string ma_doi_tac { get; set; }
        public string ma_chi_nhanh { get; set; }
        public string nsd { get; set; }
        public string pas { get; set; }
        public string pas_cu { get; set; }
        public string pas_moi { get; set; }
        public string pas_nhap_lai { get; set; }
    }
    public class user_get_pass
    {
        public string t { get; set; }
        public string signature { get; set; }
        [DataType(DataType.Password)]
        public string mat_khau_moi { get; set; }
        [DataType(DataType.Password)]
        public string nhap_lai_mat_khau { get; set; }
        public string captcha { get; set; }
        public string trang_thai { get; set; }
        public string thong_bao { get; set; }
        public user_get_pass()
        {
            this.trang_thai = "200";
        }
    }
    public class escs_nsd_quen_mk
    {
        public string ma_doi_tac { get; set; }
        public string ma_chi_nhanh { get; set; }
        public string ma { get; set; }
        public decimal? tg_yeu_cau { get; set; }
        public string email_nhan { get; set; }
        public decimal? tg_ket_thuc { get; set; }
        public string token { get; set; }
        public string trang_thai { get; set; }
    }
}
