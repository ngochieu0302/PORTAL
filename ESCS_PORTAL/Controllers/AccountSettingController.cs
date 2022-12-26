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
namespace ESCS.Controllers
{
    [SystemAuthen]
    public class AccountSettingController : BaseController
    {
        [ESCSDescription(ESCSMethod.GET, "Màn hình tìm kiếm/xem thông tin chung")]
        public IActionResult Index()
        {
            return View();
        }
    }
}