//Created:05/09/2022
//Author: thanhnx.escs
namespace ESCS_PORTAL.COMMON.ESCSStoredProcedures
{
	using System;
	using System.Collections.Generic;

	public partial class StoredProcedure
	{
		/// <summary>
		/// [PORTAL]- Cài đặt ứng dụng
		/// </summary>
		public const string PORTAL_CAI_DAT_UNG_DUNG_LKE = "2OQ2MWN9W440MSS";
		/// <summary>
		/// [PORTAL] - NSD login
		/// </summary>
		public const string PORTAL_NSD_LOGIN = "6DDAJB0JTZG957Z";
		/// <summary>
		/// [PORTAL] - Liệt kê + phân trang hợp đồng con người
		/// </summary>
		public const string PORTAL_BH_HD_NG_GCN_LKE = "CGD9T4B09FYI1PQ";
		/// <summary>
		/// [PORTAL] - Lấy danh sách đối tác CACHE
		/// </summary>
		public const string PORTAL_HT_MA_DOI_TAC_CACHE = "TUJOTW0A9AXRE0L";
		/// <summary>
		/// [PORTAL] - Lấy danh sách chi nhánh CACHE
		/// </summary>
		public const string PORTAL_HT_MA_DOI_TAC_CHI_NHANH_CACHE = "JP5KI140DV825OW";
		/// <summary>
		/// [PORTAL] - Lấy chi tiết hợp đồng con người
		/// </summary>
		public const string PORTAL_BH_HD_NG_GCN_DS_LKE = "CS8I8ZFRJKSAKL8";
		/// <summary>
		/// [PORTAL] - Lấy chi tiết ảnh
		/// </summary>
		public const string PORTAL_BH_FILE_TAI_FILE = "IRR1QZ193GOMQAJ";
		/// <summary>
		/// [PORTAL] - Lấy danh sách file thumnail
		/// </summary>
		public const string PORTAL_BH_FILE_THUMNAIL = "JSKK6ZVEHX54ABY";
		/// <summary>
		/// [PORTAL] - Lấy chi tiết quyền lợi gói bảo hiểm GCN
		/// </summary>
		public const string PORTAL_BH_HD_NG_GCN_DS_QL_LKE = "AVJZV9CNJ1HWCZ3";
		/// <summary>
		/// [PORTAL] - Lấy danh sách đồng tái của hồ sơ
		/// </summary>
		public const string PORTAL_BH_HD_NG_GCN_DS_DONG_TAI_LKE = "0DZGP3TSRLYWAZG";
		/// <summary>
		/// [PORTAL] - Lấy chi tiết đồng tái
		/// </summary>
		public const string PORTAL_BH_HD_NG_GCN_DONG_TAI_CT = "NC0F01MWMJB07W4";
		/// <summary>
		/// [PORTAL] - Lấy danh sách tất cả nhà bảo hiểm
		/// </summary>
		public const string PORTAL_BH_HT_MA_NHA_BH_TATCA = "M4GTSWOB3NVUFAV";
		/// <summary>
		/// [PORTAL] - Lấy danh sách hợp đồng
		/// </summary>
		public const string PORTAL_BH_HD_NG_GCN_DS_HDBS = "8GRFGMI43E2Y0UO";
        /// <summary>
		/// Portal tìm kiếm phân trang HSBT con người
		/// </summary>
		public const string PORTAL_BH_BT_NG_TINH_TOAN_LKE = "7BRZ0ACUNW7J4VH";
        /// <summary>
		/// [PORTAL] - Toàn bộ thông tin hồ sơ con người
		/// </summary>
		public const string PORTAL_NG_TOAN_BO_THONG_TIN_HO_SO = "B3B3RYFOFB0RB0H";
		/// <summary>
		/// [PORTAL] - Liệt kê thông tin hợp đồng xe 
		/// </summary>
		public const string PORTAL_BH_HD_XE_GCN_LKE = "5ZIX08WM0BTN0YJ";
		/// <summary>
		/// [PORTAL] - Lấy chi tiết hợp đồng
		/// </summary>
		public const string PORTAL_BH_HD_XE_GCN_DS_LKE = "EKU1WVFH8WAYLNX";
		/// <summary>
		/// [PORTAL] - Liệt kê danh sách hồ sơ giám định
		/// </summary>
		public const string PORTAL_BH_BT_XE_GD_LKE = "3YSIKKHZRANFW1P";
		/// <summary>
		/// [PORTAL] - Toàn bộ thông tin hồ sơ bồi thường xe
		/// </summary>
		public const string PORTAL_XE_TOAN_BO_THONG_TIN_HO_SO = "U2L6YXCE11R95XH";
		/// <summary>
		/// [PORTAL] - Đổi mật khẩu
		/// </summary>
		public const string PORTAL_NSD_DOI_MAT_KHAU = "QZVS79QO4QK3YWS";
        /// <summary>
		/// [PORTAL] - Lấy ds biểu mẫu
		/// </summary>
		public const string PORTAL_MAU_IN_DS_BIEU_MAU = "B48BNZSQY3YR8P4";
        /// <summary>
        /// [PORTAL] - lay DSCSYT
        /// </summary>
        public const string PORTAL_BH_HT_MA_BENH_VIEN_CACHE = "UT42OE8LBOHP2MK";
        /// <summary>
        /// [PORTAL] lấy danh mục chung
        /// </summary>
        public const string PORTAL_BH_BT_NG_MA_DANH_MUC_CACHE = "KCF86ZY1S0CEJ25";
        /// <summary>
        /// [PORTAL] - Lấy tất cả danh sách sản phẩm con người
        /// </summary>
        public const string PORTAL_HT_MA_LHNV_CN_DMUC = "ZB7BOMJF47IWZ9R";
        /// <summary>
        /// [PORTAL] - Lấy danh sách các control ẩn hiện
        /// </summary>
        public const string PORTAL_BH_BT_TRANG_THAI_TEN_AN_HIEN = "ABVW5MXDFM09D38";
        /// <summary>
        /// [PORTAL] - Cache danh mục chung theo đối tác
        /// </summary>
        public const string PORTAL_BH_BT_MA_DANH_MUC_CACHE = "J1V7TA3YNFN4DWC";
        /// <summary>
        /// Lấy thông tin mẫu in
        /// </summary>
        public const string PHT_MAU_IN_LKE_IN = "1802WYU03EV3A61";
		/// <summary>
		/// Lấy thông tin giấy chứng nhận
		/// </summary>
		public const string PORTAL_BH_HD_NGUOI_DS_LKE_CT = "IQSDLS201W1LLKU";
		/// <summary>
		/// Lấy danh sách tỉnh thành quận huyện
		/// </summary>
		public const string PORTAL_BH_HT_MA_TINH_DMUC = "K795MM9NGVZQMCL";
		/// <summary>
		/// Lấy danh sách ngân hàng/chi nhánh ngân hàng
		/// </summary>
		public const string PORTAL_BH_HT_MA_NGAN_HANG_DMUC = "14GURGZG7PZR1LU";
		/// <summary>
		/// Lưu hồ sơ bồi thường
		/// </summary>
		public const string PORTAL_KH_BT_NG_HS_TIEP_NHAN_NH = "8H5F13SRFVV95C8";
	}
}