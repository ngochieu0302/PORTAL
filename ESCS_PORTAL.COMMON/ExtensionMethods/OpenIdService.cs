using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Http;
using ESCS_PORTAL.COMMON.Request;
using ESCS_PORTAL.COMMON.Response;
using Microsoft.AspNetCore.Http;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.COMMON.ExtensionMethods
{
    public static class OpenIdService
    {
        /// <summary>
        /// Lấy thông tin DefineInfo
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public static DefineInfo GetDefineInfo(this HttpRequest rq)
        {
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            return defineInfo;
        }
        /// <summary>
        /// Excute Dynamic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionCode"></param>
        /// <param name="jsonData"></param>
        /// <param name="defineInfo"></param>
        /// <returns></returns>
        public static async Task<object> ExcuteDynamic<T>(string actionCode, string jsonData, DefineInfo defineInfo)
        {
            IHttpService _service = new HttpService();
            var rp = await _service.PostJsonAsync(actionCode, HttpConfiguration.AccessToken, jsonData, defineInfo, "/api/esmartclaim/excute");
            return rp.Result();
        }
        /// <summary>
        /// Excute Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionCode"></param>
        /// <param name="jsonData"></param>
        /// <param name="defineInfo"></param>
        /// <returns></returns>
        public static async Task<BaseResponse<T>> Excute<T>(string actionCode, string jsonData, DefineInfo defineInfo)
        {
            IHttpService _service = new HttpService();
            var rp = await _service.PostJsonAsync(actionCode, HttpConfiguration.AccessToken, jsonData, defineInfo, "/api/esmartclaim/excute");
            return rp.Result<T>();
        }
        /// <summary>
        /// Excute Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionCode"></param>
        /// <param name="jsonData"></param>
        /// <param name="defineInfo"></param>
        /// <returns></returns>
        public static async Task<BaseResponse<T>> UploadFileToPath<T>(string jsonData, DefineInfo defineInfo)
        {
            IHttpService _service = new HttpService();
            var rp = await _service.PostJsonAsync("", HttpConfiguration.AccessToken, jsonData, defineInfo, "/api/esmartclaim/upload-file-path");
            return rp.Result<T>();
        }
        /// <summary>
        /// Type File - Ký số PDF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionCode"></param>
        /// <param name="jsonData"></param>
        /// <param name="defineInfo"></param>
        /// <returns></returns>
        public static async Task<BaseResponse<file_result>> CreateAndSignatureFile(string actionCode, string jsonData, DefineInfo defineInfo)
        {
            IHttpService _service = new HttpService();
            var rp = await _service.PostJsonAsync(actionCode, HttpConfiguration.AccessToken, jsonData, defineInfo, "/api/esmartclaim/create-signature-file");
            return rp.Result<file_result>();
        }
        public static async Task<BaseResponse<file_result>> CreateSaveFile(string actionCode, string jsonData, DefineInfo defineInfo)
        {
            IHttpService _service = new HttpService();
            var rp = await _service.PostJsonAsync(actionCode, HttpConfiguration.AccessToken, jsonData, defineInfo, "/api/esmartclaim/create-save-file");
            return rp.Result<file_result>();
        }
        public static async Task<BaseResponse<file_result>> RemoveFile(string actionCode, string jsonData, DefineInfo defineInfo)
        {
            IHttpService _service = new HttpService();
            var rp = await _service.PostJsonAsync(actionCode, HttpConfiguration.AccessToken, jsonData, defineInfo, "/api/esmartclaim/remove-file");
            return rp.Result<file_result>();
        }
        public static async Task<BaseResponse<file_result>> SignatureFile(string actionCode, string jsonData, DefineInfo defineInfo)
        {
            IHttpService _service = new HttpService();
            var rp = await _service.PostJsonAsync(actionCode, HttpConfiguration.AccessToken, jsonData, defineInfo, "/api/esmartclaim/signature-file");
            return rp.Result<file_result>();
        }
        /// <summary>
        /// Forward Email
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<object> ForwardMail(MailOpenIdConfig mail, DefineInfo defineInfo)
        {
            RequestModel<MailOpenIdConfig> config = new RequestModel<MailOpenIdConfig>();
            config.define_info = defineInfo;
            config.data_info = mail;
            HttpClient service = new HttpClient();
            service.BaseAddress = new Uri(HttpConfiguration.BaseUrl);
            service.DefaultRequestHeaders.Clear();
            service.DefaultRequestHeaders.Add("ePartnerCode", HttpConfiguration.PartnerCode);
            service.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            service.DefaultRequestHeaders.Add("eAuthToken", HttpConfiguration.AccessToken);
            service.DefaultRequestHeaders.Add("eAction", "FORWARDMAIL");
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(JsonConvert.SerializeObject(config.data_info));
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            var httpContent = new StringContent(JsonConvert.SerializeObject(config), Encoding.UTF8, "application/json");
            var res =  await service.PostAsync("/api/p/esmartclaim/forward-mail", httpContent);
            return res.Result();
        }
    }
    public class file_result
    {
        public byte[] file { get; set; }
        public string path { get; set; }
    }
}
