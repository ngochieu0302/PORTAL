using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Request
{
    public class MessageNotify
    {

        public string connectionid { get; set; }
        /// <summary>
        /// Mã đối tác -> Gửi cho tất cả user của đối tác
        /// </summary>
        public string ma_doi_tac { get; set; }
        /// <summary>
        /// Người sử dụng - Gửi cho 1 cá nhân nào đó của đối tác
        /// </summary>
        public string nsd { get; set; }
        /// <summary>
        /// Mã đối tác của hồ sơ
        /// </summary>
        public string ma_doi_tac_hs { get; set; }
        /// <summary>
        /// Số id của hồ sơ
        /// </summary>
        public string so_id_hs { get; set; }
        /// <summary>
        /// Nhóm - Gửi cho 1 nhóm của đối tác
        /// 1 - gửi theo đơn vị, chi nhánh, phòng, người dùng
        /// 0 - gửi theo nhóm
        /// </summary>
        public string nhom { get; set; }
        public string signature { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung { get; set; }
    }
}
