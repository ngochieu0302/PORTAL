using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.Attributes;
using ESCS_PORTAL.COMMON.Auth;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.MODEL.ESCS_PORTAL.ModelView;
using Microsoft.AspNetCore.Http;
using ESCS_PORTAL.COMMON.Contants;
using Newtonsoft.Json;
using ESCS_PORTAL.COMMON.ESCSStoredProcedures;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.Common;

namespace ESCS_PORTAL.Controllers
{
    [SystemAuthen]
    public class DemoController : Controller
    {
        public DemoController()
        {
        }
        public IActionResult Index()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> thanhtest()
        {
            //var data = await Request.GetRespone("NVUTFOWJ83B8NEG");
            escs_nguoi_dung nd = new escs_nguoi_dung();
            nd.ma_doi_tac = "CTYBHABC";
            nd.ma_chi_nhanh = "000";
            nd.nsd = "admin@ESCS_PORTAL.vn";
            nd.pas = "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b";


            var json = Request.GetDataRequestNew(nd);
            var data = await Request.GetResponeNew("ABC", json);
            return Json(data);
        }
    }
}
