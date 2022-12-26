using ESCS_PORTAL.COMMON.Contants;
using ESCS_PORTAL.COMMON.ExceptionHandlers;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.MODEL.ESCS_PORTAL.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ESCS_PORTAL.COMMON.ESCSStoredProcedures;
using ESCS_PORTAL.MODEL.ESCS;
using ESCS_PORTAL.Common;
using ESCS_PORTAL.COMMON.Http;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Response;

namespace ESCS_PORTAL.Attributes
{
    public class SystemAuthen : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Lấy thông tin cài đặt
            LayThongTinCaiDat(context);
            string json = context.HttpContext.Request.Cookies[ESCSConstants.ESCS_TOKEN]?.ToString();
            var controller = context.Controller as Controller;
            if (string.IsNullOrEmpty(json))
                json = controller.RouteData.Values[ESCSConstants.ESCS_TOKEN]?.ToString();

            if (string.IsNullOrEmpty(json))
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.HttpContext.Response.StatusCode = 428;
                    context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại." }), Encoding.UTF8);
                    return;
                }
                var url = context.HttpContext.Request.Path.Value + "/" + context.HttpContext.Request.QueryString.Value;
                controller.TempData["_backlink"] = url;
                context.Result = new RedirectResult("/dang-nhap");
                return;
            }
            else
            {
                escs_nguoi_dung user = JsonConvert.DeserializeObject<escs_nguoi_dung>(Utilities.DecryptByKey(json, AppSettings.KeyEryptData));
                if (user.time_live <= Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")))
                {
                    user.time_live = Int64.Parse(DateTime.Now.AddMinutes((double)HttpConfiguration.SessionTimeOut).ToString("yyyyMMddHHmmss"));
                    var token = Utilities.EncryptByKey(JsonConvert.SerializeObject(user), AppSettings.KeyEryptData);
                    context.HttpContext.Response.Cookies.Delete(ESCSConstants.ESCS_TOKEN);
                    context.HttpContext.Response.Cookies.Append(ESCSConstants.ESCS_TOKEN, token, new CookieOptions() { Expires = DateTime.Now.AddMinutes(HttpConfiguration.SessionTimeOut + 10) });
                }
                LayThongTinMenu(context, user);
            }    
            var url_ss = context.HttpContext.Request.Path.Value + "/" + context.HttpContext.Request.QueryString.Value;
            if (url_ss != "//" && url_ss != "" && url_ss != null && url_ss.Contains("?hashcode="))
            {
                controller.TempData["url_link"] = url_ss;
                context.Result = controller.RedirectToAction("checkHashcode", "home");
                return;
            }
            #region Cấm xóa
            if (!context.HttpContext.Request.IsAjaxRequest())
            {
                var attrib = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(NoneMenuAttribute), true).FirstOrDefault();
                if (attrib != null)
                {
                    return;
                }
            }
            #endregion
            base.OnActionExecuting(context);
        }
        private bool CheckMenu(escs_authen authen, string areas, string controller, string action)
        {
            return true;
            if (authen.menu == null || authen.menu.Count() <= 0)
            {
                return false;
            }
            var url_action = "";
            if (!string.IsNullOrEmpty(areas))
                url_action += ("/" + areas + "/" + controller + "/" + action).ToLower();
            else
                url_action += ("/" + controller + "/" + action).ToLower();
            return authen.menu.Where(n => !string.IsNullOrEmpty(n.url) && (n.url.ToLower() == url_action || (n.url + "/index").ToLower() == url_action)).Count() > 0;
        }
        private void LayThongTinCaiDat(ActionExecutingContext context)
        {
            var domainName = context.HttpContext.Request.Scheme + "://" + context.HttpContext.Request.Host.Value.ToString().ToLower();
            if (domainName.Contains("localhost") || domainName.Contains(AppSettings.AppDomain))
                domainName = AppSettings.AppDomainLive;
            if (EscsUtils.cai_dat == null || EscsUtils.cai_dat.Count()<=0 || EscsUtils.cai_dat.Where(n=>n.domain == domainName).Count()<=0)
            {
                
                var baseResponse = context.HttpContext.Request.GetRespone<ht_cai_dat>(StoredProcedure.PORTAL_CAI_DAT_UNG_DUNG_LKE, new { domain = domainName }).Result;
                if (baseResponse.data_info!=null && baseResponse.data_info.doi_tac != null && !string.IsNullOrEmpty(baseResponse.data_info.doi_tac.ma))
                {
                    baseResponse.data_info.domain = domainName;
                    if (EscsUtils.cai_dat==null)
                        EscsUtils.cai_dat = new List<ht_cai_dat>();
                    EscsUtils.cai_dat.Add(baseResponse.data_info);
                }
                    
            }
        }
        private void LayThongTinMenu(ActionExecutingContext context, escs_nguoi_dung user)
        {
            var menu = EscsUtils.GetMenu(user.ma_doi_tac + "/" + user.nsd);
            if (menu == null || menu.Count() <=0)
            {
                var baseResponse = context.HttpContext.Request.GetRespone<escs_authen>(StoredProcedure.PORTAL_NSD_LOGIN, new { ma_doi_tac_nsd = user.ma_doi_tac, nsd = user.nsd, pas = user.pas }).Result;
                if (baseResponse.state_info.status == ResponseStatus.OK && baseResponse.data_info.nguoi_dung != null)
                    EscsUtils.SaveUserMenu(user.ma_doi_tac + "/" + user.nsd, baseResponse.data_info.menu);
            }
        }
    }
}
