using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESCS_PORTAL.COMMON.ESCSStoredProcedures;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.Attributes;
using Microsoft.AspNetCore.Mvc;
using ESCS_PORTAL.MODEL.ESCS;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ESCS_PORTAL.COMMON.Common;
using System.Web;
using Microsoft.AspNetCore.Html;
using System.Text;
using System.Linq;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.COMMON.Http;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

namespace ESCS_PORTAL.Controllers
{
    [SystemAuthen]
    public class CommonController : BaseController
    {
        private TemplateServiceConfiguration config;
        public static IRazorEngineService _service = null;
        private readonly IWebHostEnvironment _env;
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="env"></param>
        public CommonController(IWebHostEnvironment env)
        {
            _env = env;
            config = new TemplateServiceConfiguration();
            config.CachingProvider = new RazorEngine.Templating.DefaultCachingProvider();
            if (_service == null)
                _service = RazorEngineService.Create(config);
        }
        ///// <summary>
        ///// In mẫu in (Html)
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> PrintHtmlPdf()
        //{
        //    var json = Request.GetDataRequestNew(GetUser());
        //    var objData = await Request.GetResponeNew<ht_mau_in>(StoredProcedure.PHT_MAU_IN_LKE_IN, json);
        //    if (objData.data_info == null)
        //        throw new Exception("Không tìm thấy mẫu in");
        //    json = json.AddPropertyStringJson("url_file", objData.data_info.url_file);
        //    var response = await Request.GeneratePdfFile(objData.data_info.ma_action_api, json, "/api/esmartclaim/gen-html-pdf");
        //    try
        //    {
        //        var res = response.Result<object>();
        //        if (res.state_info.status == "NotOK")
        //            return Ok(res);
        //    }
        //    catch
        //    {

        //    }
        //    Stream receiveStream = await response.Content.ReadAsStreamAsync();
        //    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
        //    string html = readStream.ReadToEnd();
        //    return Ok(html);
        //}
        /// <summary>
        /// Export Excel
        /// </summary>
        /// <returns></returns>
        //public async Task<IActionResult> ExportExcel()
        //{
        //    var json = Request.GetDataRequestNew(GetUser());
        //    var objData = await Request.GetResponeNew<ht_mau_in>(StoredProcedure.PHT_MAU_IN_LKE_IN, json);
        //    json = json.AddPropertyStringJson("url_file", objData.data_info.url_file);
        //    var file = await Request.ExportExcel(objData.data_info.ma_action_api, json);
        //    try
        //    {
        //        var res = file.Result<object>();
        //        if (res.state_info.status == "NotOK")
        //            return Ok(res);
        //    }
        //    catch
        //    {

        //    }
        //    return Ok(file.Content.ReadAsByteArrayAsync().Result);
        //}
        /// <summary>
        /// Export Excel Table
        /// </summary>
        /// <returns></returns>
        //public async Task<IActionResult> ExportExcelTable()
        //{
        //    var json = Request.GetDataRequestNew(GetUser());
        //    var objData = await Request.GetResponeNew<ht_mau_in>(StoredProcedure.PHT_MAU_IN_LKE_IN, json);
        //    var file = await Request.ExportExcel(objData.data_info.ma_action_api, json, "/api/esmartclaim/export-excel-table");
        //    try
        //    {
        //        var res = file.Result<object>();
        //        if (res.state_info.status == "NotOK")
        //            return Ok(res);
        //    }
        //    catch
        //    {

        //    }
        //    return Ok(file.Content.ReadAsByteArrayAsync().Result);
        //}
    }
}
