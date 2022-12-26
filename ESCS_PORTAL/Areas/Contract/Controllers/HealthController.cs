using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCS_PORTAL.COMMON.ESCSStoredProcedures;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.Attributes;
using ESCS_PORTAL.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESCS_PORTAL.Models;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using ESCS_PORTAL.Common;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.COMMON.Http;
using ESCS_PORTAL.COMMON.Auth;
using ESCS_PORTAL.COMMON.Request;
using System.Net.Http;
using System.Net.Http.Headers;
using ESCS_PORTAL.COMMON.Common;
using System.Text;
using ESCS_PORTAL.MODEL.ESCS.ModelView;
using ESCS_PORTAL.MODEL.ESCS;
using System.Drawing;
using ESCS_PORTAL.COMMON.Contants;
namespace ESCS.Areas.Contract.Controllers
{
    [Area("Contract")]
    [SystemAuthen]
    [ESCSDescription(ESCSMethod.GET, "Hợp đồng bảo hiểm sức khỏe con người")]
    public class HealthController : BaseController
    {
        [ESCSDescription(ESCSMethod.GET, "Màn hình tìm kiếm/xem thông tin chung")]
        public IActionResult Index()
        {
            return View();
        }
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPaging()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HD_NG_GCN_LKE, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> GetDetail()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HD_NG_GCN_DS_LKE, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> GetFiles()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var dataRQ = JsonConvert.DeserializeObject<data_get_list_file>(json);
            string urlApi = "/api/esmartclaim/get-file";
            if (AppSettings.ConnectApiCorePartner && !string.IsNullOrEmpty(dataRQ.pm) && dataRQ.pm == "API")
                urlApi = "/api/partner/get-file";
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_FILE_TAI_FILE, json, urlApi);
            return Json(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> GetFilesThumnail()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var dataRQ = JsonConvert.DeserializeObject<data_get_list_file>(json);
            string urlApi = "/api/esmartclaim/get-file-thumnail";
            if (AppSettings.ConnectApiCorePartner && !string.IsNullOrEmpty(dataRQ.pm) && dataRQ.pm == "BH")
                urlApi = "/api/partner/list-file";
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_FILE_THUMNAIL, json, urlApi);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> layQuyenLoiGoiBaoHiem()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HD_NG_GCN_DS_QL_LKE, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> layDanhSachDongTai()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HD_NG_GCN_DS_DONG_TAI_LKE, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> layChiTietDongTai()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HD_NG_GCN_DONG_TAI_CT, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> layDanhSachNhaBaoHiem()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HT_MA_NHA_BH_TATCA, json);
            return Ok(data);
        }  
        [AjaxOnly]
        public async Task<IActionResult> layDanhSachHopDong()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HD_NG_GCN_DS_HDBS, json);
            return Ok(data);
        }
        /// <summary>
        /// Xem thông tin chi tiết GCN
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        public async Task<IActionResult> layThongTinGCN()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_HD_NGUOI_DS_LKE_CT, json);
            return Ok(data);
        }
        /// <summary>
        /// Mở hồ sơ bồi thường
        /// </summary>
        /// <returns></returns>
        [AjaxOnly]
        public async Task<IActionResult> moHoSoBT()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_KH_BT_NG_HS_TIEP_NHAN_NH, json);
            return Ok(data);
        }
    }
}