using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ESCS_PORTAL.COMMON.Http;
using System.Net;
using ESCS_PORTAL.COMMON.Request;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using ESCS_PORTAL.MODEL.ESCS;

namespace ESCS_PORTAL.Controllers
{
    [SystemAuthen]
    public class ViewImagesController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpService _httpService;
        private TemplateServiceConfiguration config;
        public static IRazorEngineService _service = null;
        public ViewImagesController(IWebHostEnvironment hostingEnvironment, IHttpService httpService)
        {
            _httpService = httpService;
            _hostingEnvironment = hostingEnvironment;
            config = new TemplateServiceConfiguration();
            config.CachingProvider = new RazorEngine.Templating.DefaultCachingProvider();
            if (_service == null)
                _service = RazorEngineService.Create(config);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
