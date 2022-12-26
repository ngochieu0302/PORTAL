using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.ESCS
{
    public class ht_cai_dat
    {
        public ma_doi_tac doi_tac { get; set; }
        public string domain { get; set; }
        public List<bh_cai_dat_ung_dung> cai_dat { get; set; }
        public ht_cai_dat()
        {
            doi_tac = new ma_doi_tac();
            cai_dat = new List<bh_cai_dat_ung_dung>();
        }
    }
}
