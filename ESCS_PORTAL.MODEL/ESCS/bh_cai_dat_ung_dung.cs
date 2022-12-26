using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.ESCS
{
    public class bh_cai_dat_ung_dung
    {
        public string ma_doi_tac { get; set; }
        public string ma_app { get; set; }
        public string url_anh { get; set; }
        public string ten { get; set; }
        public string ten_tat { get; set; }
        public string loai { get; set; }
        public string domain { get; set; }
        public Nullable<decimal> chieu_dai { get; set; }
        public Nullable<decimal> chieu_rong { get; set; }
        public Nullable<decimal> toa_do_x { get; set; }
        public Nullable<decimal> toa_do_y { get; set; }
        public string ten_doi_tac { get; set; }
        public string ten_tat_doi_tac { get; set; }
    }
}
