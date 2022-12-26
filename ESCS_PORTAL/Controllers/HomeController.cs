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
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ESCS_PORTAL.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpService _httpService;
        private TemplateServiceConfiguration config;
        public static IRazorEngineService _service = null;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IWebHostEnvironment hostingEnvironment, IHttpService httpService, ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpService = httpService;
            _hostingEnvironment = hostingEnvironment;
            config = new TemplateServiceConfiguration();
            config.CachingProvider = new RazorEngine.Templating.DefaultCachingProvider();
            if (_service == null)
                _service = RazorEngineService.Create(config);
        }
        /// <summary>
        /// Trang chủ
        /// </summary>
        /// <returns></returns>
        [SystemAuthen]
        [NoneMenu]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// IndexH
        /// </summary>
        /// <returns></returns>
        [SystemAuthen]
        [NoneMenu]
        public IActionResult IndexH()
        {
            return View("Error");
        }
        /// <summary>
        /// IndexTTGQ
        /// </summary>
        /// <returns></returns>
        [SystemAuthen]
        [NoneMenu]
        public IActionResult IndexTTGQ()
        {
            return View();
        }
        /// <summary>
        /// Chi tiết dashboard
        /// </summary>
        /// <returns></returns>
        /// 

        [SystemAuthen]
        [NoneMenu]
        public IActionResult Export()
        {
            return View();
        }

        [SystemAuthen]
        [NoneMenu]
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult GetCheckSum(string merchant_secret, string ma_dvi, string ma_chi_nhanh, long? so_id_hs)
        {
            string chuoi = merchant_secret + ma_dvi + ma_chi_nhanh + (so_id_hs == null ? "" : ((long)so_id_hs).ToString());
            string hmacSha1 = Utilities.HMACSHA1(chuoi, merchant_secret);
            return Ok(hmacSha1.Replace("=", "%3d").Replace(" ", "+"));
        }
        /// <summary>
        /// Trang login
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Login()
        {
            var domainName = Request.Scheme + "://" + Request.Host.Value.ToString().ToLower();
            if (domainName.Contains("localhost") || domainName.Contains(AppSettings.AppDomain))
                domainName = AppSettings.AppDomainLive;
            if (AppSettings.WriteLogFile)
                _logger.LogInformation(domainName);
            ViewBag.domain = domainName;

            if (EscsUtils.cai_dat == null || EscsUtils.cai_dat.Count() <= 0 || EscsUtils.cai_dat.Where(n => n.domain == domainName).Count() <= 0)
            {
                var baseResponse = await Request.GetRespone<ht_cai_dat>(StoredProcedure.PORTAL_CAI_DAT_UNG_DUNG_LKE, new { domain = domainName });
                if (baseResponse.data_info != null && baseResponse.data_info.doi_tac != null && !string.IsNullOrEmpty(baseResponse.data_info.doi_tac.ma))
                {
                    if (EscsUtils.cai_dat == null)
                        EscsUtils.cai_dat = new List<ht_cai_dat>();
                    baseResponse.data_info.domain = domainName;
                    EscsUtils.cai_dat.Add(baseResponse.data_info);
                }
            }
            return View();
        }

        ///// <summary>
        ///// Đăng nhập hệ thống
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(user_login_escs model)
        {
            var detector = new DeviceDetector(Request.Headers["User-Agent"].ToString());
            detector.SetCache(new DictionaryCache());
            detector.Parse();

            if (AppSettings.UseCaptcha)
            {
                string captchaToken = GetCookies("SESSIONID.CAPTCHA");
                if (string.IsNullOrEmpty(captchaToken))
                {
                    ModelState.AddModelError("captcha", "Không tìm thấy captcha");
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.captcha))
                {
                    ModelState.AddModelError("captcha", "Bạn chưa nhập captcha");
                }
                string captcha = Utilities.DecryptByKey(captchaToken, "ESCS_TOKEN_KEY" + DateTime.Now.ToString("yyyyMMdd"));
                if (model.captcha != captcha)
                {
                    ModelState.AddModelError("captcha", "Mã captcha chưa chính xác");
                }
            }
            if (string.IsNullOrEmpty(model.username))
            {
                ModelState.AddModelError("username", "Bạn chưa nhập tài khoản");
            }
            if (ModelState.IsValid)
            {
                var domainName = Request.Scheme + "://" + Request.Host.Value.ToString().ToLower();
                if (domainName.Contains("localhost") || domainName.Contains(AppSettings.AppDomain))
                    domainName = AppSettings.AppDomainLive;
                ViewBag.domain = domainName;
                if (EscsUtils.cai_dat == null || EscsUtils.cai_dat.Count() <= 0 || EscsUtils.cai_dat.Where(n => n.domain == domainName).Count() <= 0)
                {
                    var resCaiDat = await Request.GetRespone<ht_cai_dat>(StoredProcedure.PORTAL_CAI_DAT_UNG_DUNG_LKE, new { domain = domainName });
                    if (resCaiDat.data_info != null && resCaiDat.data_info.doi_tac != null && !string.IsNullOrEmpty(resCaiDat.data_info.doi_tac.ma))
                    {
                        if (EscsUtils.cai_dat == null)
                            EscsUtils.cai_dat = new List<ht_cai_dat>();
                        resCaiDat.data_info.domain = domainName;
                        EscsUtils.cai_dat.Add(resCaiDat.data_info);
                    }
                }
                var ma_doi_tac_tmp = EscsUtils.cai_dat.Where(n => n.domain == domainName).FirstOrDefault().doi_tac.ma;
                var baseResponse = await Request.GetRespone<escs_authen>(StoredProcedure.PORTAL_NSD_LOGIN, new { ma_doi_tac_nsd = ma_doi_tac_tmp, nsd = model.username, pas = Utilities.Sha256Hash(model.password)});
                if (baseResponse.state_info.status == ResponseStatus.OK && baseResponse.data_info.nguoi_dung != null)
                {
                    baseResponse.data_info.nguoi_dung.pas = Utilities.Sha256Hash(model.password);
                    EscsUtils.SaveUserMenu(baseResponse.data_info.nguoi_dung.ma_doi_tac + "/" + model.username, baseResponse.data_info.menu);

                    baseResponse.data_info.nguoi_dung.time_live = Int64.Parse(DateTime.Now.AddMinutes((double)HttpConfiguration.SessionTimeOut).ToString("yyyyMMddHHmmss"));
                    var token = Utilities.EncryptByKey(JsonConvert.SerializeObject(baseResponse.data_info.nguoi_dung), AppSettings.KeyEryptData);

                    HttpContext.Response.Cookies.Delete(ESCSConstants.ESCS_TOKEN);
                    HttpContext.Response.Cookies.Append(ESCSConstants.ESCS_TOKEN, token, new CookieOptions() { Expires = DateTime.Now.AddMinutes(HttpConfiguration.SessionTimeOut + 10) });
                    TempData[ESCSConstants.ESCS_TOKEN] = token;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("username", baseResponse.state_info.message_body);
            }
            return View(model);
        }

        ///// <summary>
        ///// Trang lấy lại mật khẩu
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> GetPass(string t, string signature)
        //{
        //    user_get_pass userpass = new user_get_pass();
        //    userpass.t = t;
        //    userpass.signature = signature;
        //    if (userpass == null ||
        //        string.IsNullOrEmpty(userpass.t) ||
        //        string.IsNullOrEmpty(userpass.signature) ||
        //        Utilities.Sha256Hash(userpass.t + AppSettings.SecretKeyData + DateTime.Now.ToString("yyyyMMdd")) != userpass.signature)
        //    {
        //        userpass.trang_thai = "500";
        //        userpass.thong_bao = "Thông tin không hợp lệ";
        //        return View(userpass);
        //    }
        //    var baseResponse = await Request.GetRespone<escs_nsd_quen_mk>(StoredProcedure.PHT_NSD_QUEN_MK_LKE_CT, new { token = userpass.t });
        //    if (baseResponse.state_info.status != STATUS_OK)
        //    {
        //        userpass.trang_thai = "500";
        //        userpass.thong_bao = baseResponse.state_info.message_body;
        //        return View(userpass);
        //    }
        //    if (baseResponse.data_info == null)
        //    {
        //        userpass.trang_thai = "500";
        //        userpass.thong_bao = "Thông tin không hợp lệ";
        //        return View(userpass);
        //    }
        //    if (baseResponse.data_info.tg_ket_thuc < Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss")))
        //    {
        //        userpass.trang_thai = "500";
        //        userpass.thong_bao = "Thời gian thay đổi mật khẩu đã hết hạn (" + AppSettings.TimeRecoverPass + " phút). Vui lòng thử lại lần sau.";
        //        return View(userpass);
        //    }
        //    userpass.trang_thai = "200";
        //    return View(userpass);
        //}
        /////// <summary>
        /////// Trang lấy lại mật khẩu
        /////// </summary>
        /////// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> GetPass(user_get_pass userpass)
        //{
        //    if (userpass == null ||
        //        string.IsNullOrEmpty(userpass.t) ||
        //        string.IsNullOrEmpty(userpass.signature) ||
        //        Utilities.Sha256Hash(userpass.t + AppSettings.SecretKeyData + DateTime.Now.ToString("yyyyMMdd")) != userpass.signature)
        //    {
        //        userpass.trang_thai = "500";
        //        userpass.thong_bao = "Thông tin không hợp lệ";
        //        return View(userpass);
        //    }
        //    if (string.IsNullOrEmpty(userpass.mat_khau_moi))
        //    {
        //        ModelState.AddModelError("mat_khau_moi", "Bạn chưa nhập mật khẩu mới");
        //    }
        //    if (string.IsNullOrEmpty(userpass.nhap_lai_mat_khau))
        //    {
        //        ModelState.AddModelError("nhap_lai_mat_khau", "Bạn chưa nhập lại mật khẩu mới");
        //    }
        //    if (userpass.mat_khau_moi != userpass.nhap_lai_mat_khau)
        //    {
        //        ModelState.AddModelError("nhap_lai_mat_khau", "Nhập lại mật khẩu mới không trùng nhau");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var baseResponse = await Request.GetRespone<escs_nsd_quen_mk>(StoredProcedure.PHT_NSD_QUEN_MK_LKE_CT, new { token = userpass.t });
        //        if (baseResponse.state_info.status != STATUS_OK)
        //        {
        //            userpass.trang_thai = "500";
        //            userpass.thong_bao = baseResponse.state_info.message_body;
        //            return View(userpass);
        //        }
        //        if (baseResponse.data_info == null)
        //        {
        //            userpass.trang_thai = "500";
        //            userpass.thong_bao = "Thông tin không hợp lệ";
        //            return View(userpass);
        //        }
        //        if (baseResponse.data_info.tg_ket_thuc < Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss")))
        //        {
        //            userpass.trang_thai = "500";
        //            userpass.thong_bao = "Thời gian thay đổi mật khẩu đã hết hạn (" + AppSettings.TimeRecoverPass + " phút). Vui lòng thử lại lần sau.";
        //            return View(userpass);
        //        }
        //        var resCapNhatMatKhau = await Request.GetRespone<int>(StoredProcedure.PHT_NSD_CAP_LAI_MK, new { tai_khoan = baseResponse.data_info.ma, mat_khau = Utilities.Sha256Hash(userpass.mat_khau_moi) });
        //        if (resCapNhatMatKhau.state_info.status != STATUS_OK)
        //        {
        //            userpass.trang_thai = "500";
        //            userpass.thong_bao = resCapNhatMatKhau.state_info.message_body;
        //            return View(userpass);
        //        }
        //        userpass.trang_thai = "SUCCESS";
        //    }
        //    return View(userpass);
        //}
        ///// <summary>
        ///// Logout hệ thống
        ///// </summary>
        ///// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(ESCSConstants.ESCS_TOKEN);
            return RedirectToAction("Login");
        }
        ///// <summary>
        ///// Màn hình thông báo lỗi
        ///// </summary>
        ///// <returns></returns>
        [HttpGet]
        [NoneMenu]
        public ViewResult Error() => View("Error");
        ///// <summary>
        ///// Màn hình thông báo không hỗ trợ trên mobile
        ///// </summary>
        ///// <returns></returns>
        [NoneMenu]
        public IActionResult DetectDevice()
        {
            return View();
        }
        ///// <summary>
        ///// Màn hình page notfound
        ///// </summary>
        ///// <returns></returns>
        [HttpGet]
        public ViewResult PageNotFound() => View("PageNotFound");
        ///// <summary>
        ///// Captcha
        ///// </summary>
        ///// <param name="prefix"></param>
        ///// <param name="noisy"></param>
        ///// <returns></returns>
        public IActionResult Captcha(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);
            var cookie_value = Utilities.EncryptByKey((a + b).ToString(), "ESCS_TOKEN_KEY" + DateTime.Now.ToString("yyyyMMdd"));
            SetCookies("SESSIONID.CAPTCHA", cookie_value, 10);
            FileContentResult img = null;
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(System.Drawing.Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = System.Drawing.Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }
            return img;
        }
        ///// <summary>
        ///// Captcha Recover Pass
        ///// </summary>
        ///// <param name="prefix"></param>
        ///// <param name="noisy"></param>
        ///// <returns></returns>
        public IActionResult CaptchaRecoverPass(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);
            var cookie_value = Utilities.EncryptByKey((a + b).ToString(), "ESCS_TOKEN_KEY" + DateTime.Now.ToString("yyyyMMdd"));
            SetCookies("SESSIONID.CAPTCHA_RECOVER_PASS", cookie_value, 10);
            FileContentResult img = null;
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(System.Drawing.Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = System.Drawing.Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }
            return img;
        }
        ///// <summary>
        ///// Đổi mật khẩu
        ///// </summary>
        ///// <returns></returns>
        [AjaxOnly]
        public async Task<IActionResult> ChangePass()
        {
            var jsonData = Request.GetDataRequestNew(GetUser());
            user_login_escs_change_pass user = JsonConvert.DeserializeObject<user_login_escs_change_pass>(jsonData);
            BaseResponse<user_login_escs_change_pass> res = new BaseResponse<user_login_escs_change_pass>();
            if (user == null || string.IsNullOrEmpty(user.pas_cu))
            {
                res.state_info.status = "NotOK";
                res.state_info.message_code = "PAS_CU_NOTFOUND";
                res.state_info.message_body = "Bạn chưa nhập mật khẩu cũ";
                return Ok(res);
            }
            if (string.IsNullOrEmpty(user.pas_moi))
            {
                res.state_info.status = "NotOK";
                res.state_info.message_code = "PAS_MOI_NOTFOUND";
                res.state_info.message_body = "Bạn chưa nhập mật khẩu mới";
                return Ok(res);
            }
            if (string.IsNullOrEmpty(user.pas_nhap_lai))
            {
                res.state_info.status = "NotOK";
                res.state_info.message_code = "PAS_NHAP_LAI_NOTFOUND";
                res.state_info.message_body = "Bạn chưa nhập lại mật khẩu mới";
                return Ok(res);
            }
            if (user.pas_nhap_lai != user.pas_moi)
            {
                res.state_info.status = "NotOK";
                res.state_info.message_code = "PAS_NHAP_LAI_INVALID";
                res.state_info.message_body = "Nhập lại mật khẩu mới không khớp";
                return Ok(res);
            }
            user.pas_cu = Utilities.Sha256Hash(user.pas_cu);
            user.pas_moi = Utilities.Sha256Hash(user.pas_moi);
            var userSession = GetUser();
            user.ma_doi_tac_nsd = userSession.ma_doi_tac;
            user.ma_chi_nhanh_nsd = userSession.ma_chi_nhanh;
            user.pas = userSession.pas;
            user.nsd = userSession.nsd;
            var json = JsonConvert.SerializeObject(user);
            var data = await Request.GetResponeNew(StoredProcedure.PORTAL_NSD_DOI_MAT_KHAU, json);
            try
            {
                var jsonR = JsonConvert.SerializeObject(data);
                BaseResponse<object> resChangePass = JsonConvert.DeserializeObject<BaseResponse<object>>(jsonR);
                if (resChangePass.state_info.status == "OK")
                {
                    var session = HttpContext.Request.Cookies[ESCSConstants.ESCS_TOKEN]?.ToString();
                    escs_nguoi_dung auth = JsonConvert.DeserializeObject<escs_nguoi_dung>(Utilities.DecryptByKey(session, ESCS_PORTAL.COMMON.Http.AppSettings.KeyEryptData));
                    auth.pas = user.pas_moi;

                    auth.time_live = Int64.Parse(DateTime.Now.AddMinutes((double)HttpConfiguration.SessionTimeOut).ToString("yyyyMMddHHmmss"));
                    var token = Utilities.EncryptByKey(JsonConvert.SerializeObject(auth), AppSettings.KeyEryptData);

                    HttpContext.Response.Cookies.Delete(ESCSConstants.ESCS_TOKEN);
                    HttpContext.Response.Cookies.Append(ESCSConstants.ESCS_TOKEN, token, new CookieOptions() { Expires = DateTime.Now.AddMinutes(HttpConfiguration.SessionTimeOut + 10) });
                }
            }
            catch
            {

            }
            return Ok(data);
        }
        ///// <summary>
        ///// Khôi phục lại mật khẩu
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> RecoverPass(user_login_escs model)
        //{
        //    BaseResponse<string> res = new BaseResponse<string>();
        //    string pathFile = string.Empty;
        //    string template_mail = string.Empty;
        //    #region Kiểm tra dữ liệu đầu vào
        //    string captchaToken = GetCookies("SESSIONID.CAPTCHA_RECOVER_PASS");
        //    if (string.IsNullOrEmpty(captchaToken))
        //    {
        //        res.state_info.status = STATUS_NOTOK;
        //        res.state_info.message_code = "validate-recover-pass-captcha.notfound";
        //        res.state_info.message_body = "Không tìm thấy captcha";
        //        return Ok(res);
        //    }
        //    if (string.IsNullOrEmpty(model.captcha))
        //    {
        //        res.state_info.status = STATUS_NOTOK;
        //        res.state_info.message_code = "validate-recover-pass-captcha.notfound";
        //        res.state_info.message_body = "Bạn chưa nhập captcha";
        //        return Ok(res);
        //    }
        //    string captcha = Utilities.DecryptByKey(captchaToken, "ESCS_TOKEN_KEY" + DateTime.Now.ToString("yyyyMMdd"));
        //    if (model.captcha != captcha)
        //    {
        //        res.state_info.status = STATUS_NOTOK;
        //        res.state_info.message_code = "validate-recover-pass-captcha.invalid";
        //        res.state_info.message_body = "Mã captcha chưa chính xác";
        //        return Ok(res);
        //    }
        //    if (string.IsNullOrEmpty(model.username))
        //    {
        //        res.state_info.status = STATUS_NOTOK;
        //        res.state_info.message_code = "validate-recover-pass-username.notfound";
        //        res.state_info.message_body = "Bạn chưa nhập tên tài khoản";
        //        return Ok(res);
        //    }
        //    if (!string.IsNullOrEmpty(model.username) && !Utilities.IsValidEmail(model.username))
        //    {
        //        res.state_info.status = STATUS_NOTOK;
        //        res.state_info.message_code = "validate-recover-pass-username.invalid";
        //        res.state_info.message_body = "Tài khoản không đúng định dạng email.";
        //        return Ok(res);
        //    }
        //    #endregion
        //    #region Lấy dữ liệu Database
        //    var baseResponse = await Request.GetRespone<escs_quen_mat_khau<string>>(StoredProcedure.PHT_NSD_QUEN_MK, new { username = model.username });
        //    escs_quyen_mk_token value_out = null;
        //    if (baseResponse.out_value != null && !string.IsNullOrEmpty(baseResponse.out_value.ToString()))
        //    {
        //        value_out = JsonConvert.DeserializeObject<escs_quyen_mk_token>(baseResponse.out_value.ToString());
        //        baseResponse.data_info.link_lien_ket = AppSettings.AppDomain + "/home/getpass?t=" + value_out.token + "&signature=" + Utilities.Sha256Hash(value_out.token + AppSettings.SecretKeyData + DateTime.Now.ToString("yyyyMMdd"));
        //    }
        //    #endregion
        //    #region Lấy template email
        //    NetworkCredentialItem network = NetworkCredentials.GetItem("ESCS_PATH_FILE");
        //    pathFile = Path.Combine(network.PathLocal, "FILE_CAM_XOA", baseResponse.data_info.mau_email.url);
        //    if (!System.IO.File.Exists(pathFile))
        //    {
        //        res.state_info.status = STATUS_NOTOK;
        //        res.state_info.message_code = "500";
        //        res.state_info.message_body = "Không tồn tại file";
        //        return Ok(res);
        //    }
        //    template_mail = System.IO.File.ReadAllText(pathFile);
        //    #endregion
        //    var defineInfo = Request.GetDefineInfo();
        //    Task task = new Task(async () =>
        //    {
        //        #region Compile template
        //        DynamicViewBag dynamicViewBag = new DynamicViewBag();
        //        dynamicViewBag.AddValue("Data", baseResponse);
        //        string name = baseResponse.data_info.mau_email.ma_doi_tac + "_" + baseResponse.data_info.mau_email.ma;
        //        try { template_mail = _service.RunCompile(template_mail, name, null, null, dynamicViewBag); } catch (Exception ex) { throw new Exception("Lỗi cú pháp khi đổ dữ liệu: " + ex.Message); }
        //        #endregion
        //        #region Gửi Email
        //        MailOpenIdConfig mailOpenIdConfig = new MailOpenIdConfig();
        //        mailOpenIdConfig.title = "Thiết lập lại mật khẩu của tài khoản người dùng";
        //        mailOpenIdConfig.body = template_mail;
        //        //Thông tin server mail
        //        mailOpenIdConfig.server.smtp_server = baseResponse.data_info.server.smtp_server;
        //        mailOpenIdConfig.server.smtp_port = (int)baseResponse.data_info.server.smtp_port.Value;
        //        mailOpenIdConfig.server.smtp_username = baseResponse.data_info.server.smtp_tai_khoan;
        //        mailOpenIdConfig.server.smtp_password = baseResponse.data_info.server.smtp_mat_khau;
        //        //Thông tin tài khoản gửi
        //        mailOpenIdConfig.from.username = baseResponse.data_info.server.smtp_tai_khoan;
        //        mailOpenIdConfig.from.password = baseResponse.data_info.server.smtp_mat_khau;
        //        mailOpenIdConfig.from.alias = baseResponse.data_info.server.ten_hthi;
        //        mailOpenIdConfig.to.Add(new MailInfo(baseResponse.data_info.nsd.email));
        //        await OpenIdService.ForwardMail(mailOpenIdConfig, defineInfo);
        //        #endregion
        //    });
        //    task.Start();
        //    res.state_info.message_code = "200";
        //    res.state_info.message_body = "Gửi email lấy lại mật khẩu thành công";
        //    return Ok(res);
        //}
    }
}