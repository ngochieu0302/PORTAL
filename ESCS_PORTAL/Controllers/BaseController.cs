using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCS_PORTAL.Attributes;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Contants;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.MODEL.ESCS_PORTAL.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ESCS_PORTAL.Controllers
{
    public class BaseController : Controller
    {
        public const string STATUS_OK = "OK";
        public const string STATUS_NOTOK = "NotOK";
        public escs_nguoi_dung GetUser()
        {
            string session = HttpContext.Request.Cookies[ESCSConstants.ESCS_TOKEN]?.ToString();
            escs_nguoi_dung user = JsonConvert.DeserializeObject<escs_nguoi_dung>(Utilities.DecryptByKey(session, ESCS_PORTAL.COMMON.Http.AppSettings.KeyEryptData));
            return user;
        }
        public BaseResponse<object> GetResultExample(object data)
        {
            BaseResponse<object> res = new BaseResponse<object>();
            res.data_info = data;
            return res;
        }
        public void SetCookies(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        public string GetCookies(string key)
        {
            return Request.Cookies[key];
        }
        public void RemoveCookies(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
