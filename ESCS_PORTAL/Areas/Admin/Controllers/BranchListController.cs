using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.Attributes;
using ESCS_PORTAL.Controllers;
using Microsoft.AspNetCore.Mvc;
using ESCS_PORTAL.COMMON.ESCSStoredProcedures;
using System.ComponentModel;

namespace ESCS_PORTAL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [SystemAuthen]
    public class BranchListController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [AjaxOnly]
        public async Task<IActionResult> GetAll()
        {
            var json = Request.GetDataRequestNew(GetUser());
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_HT_MA_DOI_TAC_CHI_NHANH_CACHE, json);
            return Ok(data);
        }
    }
}