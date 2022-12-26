using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.ESCSStoredProcedures;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.Attributes;
using ESCS.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using ESCS_PORTAL.Common;
using ESCS_PORTAL.COMMON.Request;
using ESCS_PORTAL.MODEL.ESCS;
using Newtonsoft.Json;
using System.IO;
using ESCS_PORTAL.Controllers;
using Nancy;

namespace ESCS_PORTAL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [SystemAuthen]
    [ESCSDescription(ESCSMethod.GET, "Hợp đồng bảo hiểm sức khỏe con người")]
    public class PrintedController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [AjaxOnly]
        public async Task<IActionResult> layDanhSachBieuMauBaoCao()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_MAU_IN_DS_BIEU_MAU, json);
            return Ok(data);
        }

        [AjaxOnly]
        public async Task<IActionResult> layDSCoSoYTe()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HT_MA_BENH_VIEN_CACHE, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> layDMChungNG()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_BT_NG_MA_DANH_MUC_CACHE, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> getAllProductHuman()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_HT_MA_LHNV_CN_DMUC, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> getControl()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_BT_TRANG_THAI_TEN_AN_HIEN, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> layDMChungXE()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_BT_MA_DANH_MUC_CACHE, json);
            return Ok(data);
        }
        //Lấy danh sách tỉnh thành quận huyện
        [AjaxOnly]
        public async Task<IActionResult> layTatCaDonViHanhChinh()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HT_MA_TINH_DMUC, json);
            return Ok(data);
        }
        //Lấy danh sách ngân hàng/ chi nhánh ngân hàng
        [AjaxOnly]
        public async Task<IActionResult> layTatCaNganHang()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HT_MA_NGAN_HANG_DMUC, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> exportBaoCao()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var objData = await Request.GetResponeNew<ht_mau_in>(StoredProcedure.PHT_MAU_IN_LKE_IN, json);
            //return Ok(objData);

            json = json.AddPropertyStringJson("url_file", objData.data_info.url_file);
            var file = await Request.ExportBaoCao(objData.data_info.ma_action_api, json);
            try
            {
                var res = file.Result<object>();
                if (res.state_info.status == "NotOK")
                    return Ok(res);
            }
            catch
            {

            }
            return Ok(file.Content.ReadAsByteArrayAsync().Result);
        }
    }
}
