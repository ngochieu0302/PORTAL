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
using DocumentFormat.OpenXml.Drawing;
using System.Runtime.CompilerServices;

namespace ESCS.Areas.Compensation.Controllers
{
    [Area("Compensation")]
    [SystemAuthen]
    [ESCSDescription(ESCSMethod.GET, "Bồi thường xe cơ giới")]
    public class CarCompensationController : BaseController
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
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_BH_BT_XE_GD_LKE, json);
            return Ok(data);
        }
        [AjaxOnly]
        public async Task<IActionResult> GetDetail()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_XE_TOAN_BO_THONG_TIN_HO_SO, json);
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
    }
}