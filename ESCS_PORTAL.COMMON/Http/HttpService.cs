using ESCS_PORTAL.COMMON.Auth;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.COMMON.Request;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.COMMON.Http
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> RefreshToken(authentication model);
        Task<HttpResponseMessage> Login(account model);
        Task<HttpResponseMessage> PostAsync(string url, string accessToken, object model, DefineInfo defineInfo = null);
        Task<HttpResponseMessage> PostGetFileAsync(string url, string accessToken, object model, DefineInfo defineInfo = null);
        Task<HttpResponseMessage> PostFormDataAsync(string url, string accessToken, object model, DefineInfo defineInfo = null, IFormFileCollection files = null);
        Task<HttpResponseMessage> AttachFileEmail(string action, string accessToken, object model, DefineInfo defineInfo = null, IFormFileCollection files = null);
        Task<HttpResponseMessage> PostDataAndFileByPathAsync(string url, string action, string accessToken, string json, DefineInfo defineInfo = null, List<string> files = null);
        Task<HttpResponseMessage> PostJsonAsync(string url, string accessToken, string json, DefineInfo defineInfo = null, string type = "/api/esmartclaim/excute");
        Task<HttpResponseMessage> GetFile(string url, string accessToken, object model, DefineInfo defineInfo = null);
        Task<HttpResponseMessage> CallApi<T>(string url, BaseRequest<T> data, string actionCode = "ESCSAPI");
    }
    public class HttpService : IHttpService
    {
        public HttpClient service;
        public HttpService()
        {
            service = new HttpClient();
            service.BaseAddress = new Uri(HttpConfiguration.BaseUrl);
            service.DefaultRequestHeaders.Clear();
            service.DefaultRequestHeaders.Add("ePartnerCode", HttpConfiguration.PartnerCode);
            service.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<HttpResponseMessage> RefreshToken(authentication model)
        {
            service.DefaultRequestHeaders.Add("eSignature", "");
            return await service.PostAsJsonAsync("/api/esmartclaim/refresh-token", model);
        }
        public async Task<HttpResponseMessage> Login(account model)
        {
            service.DefaultRequestHeaders.Add("eSignature", "");
            return await service.PostAsJsonAsync("/api/esmartclaim/auth", model);
        }
        public async Task<HttpResponseMessage> PostAsync(string action, string accessToken, object model, DefineInfo defineInfo = null)
        {
            BaseRequestEscs rqBase = new BaseRequestEscs(model);
            rqBase.define_info = defineInfo;
            service.DefaultRequestHeaders.Add("eAuthToken", accessToken);
            service.DefaultRequestHeaders.Add("eAction", action);
            string json = JsonConvert.SerializeObject(model);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(json);
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            return await service.PostAsJsonAsync("/api/esmartclaim/excute", rqBase);
        }
        public async Task<HttpResponseMessage> PostGetFileAsync(string action, string accessToken, object model, DefineInfo defineInfo = null)
        {
            BaseRequestEscs rqBase = new BaseRequestEscs(model);
            rqBase.define_info = defineInfo;
            service.DefaultRequestHeaders.Add("eAuthToken", accessToken);
            service.DefaultRequestHeaders.Add("eAction", action);
            string json = JsonConvert.SerializeObject(model);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(json);
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            return await service.PostAsJsonAsync("/api/esmartclaim/get-file-thumnail", rqBase);
        }
        public async Task<HttpResponseMessage> GetFile(string action, string accessToken, object model, DefineInfo defineInfo = null)
        {
            BaseRequestEscs rqBase = new BaseRequestEscs(model);
            rqBase.define_info = defineInfo;
            service.DefaultRequestHeaders.Add("eAuthToken", accessToken);
            service.DefaultRequestHeaders.Add("eAction", action);
            string json = JsonConvert.SerializeObject(model);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(json);
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            return await service.PostAsJsonAsync("/api/esmartclaim/get-file", rqBase);
        }
        public async Task<HttpResponseMessage> PostFormDataAsync(string action, string accessToken, object model, DefineInfo defineInfo = null, IFormFileCollection files = null)
        {
            BaseRequestEscs rqBase = new BaseRequestEscs(model);
            rqBase.define_info = defineInfo;
            service.DefaultRequestHeaders.Add("eAuthToken", accessToken);
            service.DefaultRequestHeaders.Add("eAction", action);
            string json = JsonConvert.SerializeObject(model);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(json);
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                if (files != null && files.Count > 0)
                {
                    int index = 0;
                    foreach (var file in files)
                    {
                        using (var stream = file.OpenReadStream())
                        {
                            content.Add(new StreamContent(new MemoryStream(Utilities.StreamToByteArray(stream))), "file"+ index, file.FileName);
                            index++;
                        }
                    }
                }
                var stringContent = new StringContent(JsonConvert.SerializeObject(rqBase));
                content.Add(stringContent,"data");
                return await service.PostAsync("/api/esmartclaim/upload-file", content);
            }
        }


        public async Task<HttpResponseMessage> AttachFileEmail(string action, string accessToken, object model, DefineInfo defineInfo = null, IFormFileCollection files = null)
        {
            BaseRequestEscs rqBase = new BaseRequestEscs(model);
            rqBase.define_info = defineInfo;
            service.DefaultRequestHeaders.Add("eAuthToken", accessToken);
            service.DefaultRequestHeaders.Add("eAction", action);
            string json = JsonConvert.SerializeObject(model);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(json);
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                if (files != null && files.Count > 0)
                {
                    int index = 0;
                    foreach (var file in files)
                    {
                        using (var stream = file.OpenReadStream())
                        {
                            content.Add(new StreamContent(new MemoryStream(Utilities.StreamToByteArray(stream))), "file" + index, file.FileName);
                            index++;
                        }
                    }
                }
                var stringContent = new StringContent(JsonConvert.SerializeObject(rqBase));
                content.Add(stringContent, "data");
                return await service.PostAsync("/api/esmartclaim/attach-file-email", content);
            }
        }

        public async Task<HttpResponseMessage> PostDataAndFileByPathAsync(string url, string action, string accessToken, string json, DefineInfo defineInfo = null, List<string> files = null)
        {
            string jsonBase = GetJsonBaseRequest(json, defineInfo);
            service.DefaultRequestHeaders.Add("eAuthToken", accessToken);
            service.DefaultRequestHeaders.Add("eAction", action);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(json);
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);

            using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                if (files != null && files.Count > 0)
                {
                    int index = 0;
                    foreach (var path in files)
                    {
                        using (var stream = System.IO.File.Open(path,FileMode.Open))
                        {
                            content.Add(new StreamContent(new MemoryStream(Utilities.StreamToByteArray(stream))), "file" + index, Path.GetFileName(path)) ;
                        }
                    }
                }
                var stringContent = new StringContent(jsonBase);
                content.Add(stringContent, "data");
                return await service.PostAsync(url, content);
            }
        }
        public async Task<HttpResponseMessage> PostJsonAsync(string action, string accessToken, string json, DefineInfo defineInfo = null, string urlApi = "/api/esmartclaim/excute")
        {
            string jsonBase = GetJsonBaseRequest(json, defineInfo);
            service.DefaultRequestHeaders.Add("eAuthToken", accessToken);
            service.DefaultRequestHeaders.Add("eAction", action);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(json);

            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            var httpContent = new StringContent(jsonBase, Encoding.UTF8, "application/json");
            return await service.PostAsync(urlApi, httpContent);
        }
        public string GetJsonBaseRequest(string json_data_info, DefineInfo defineInfo)
        {
            if (string.IsNullOrEmpty(json_data_info))
            {
                json_data_info = "{}";
            }
            var sb = new StringBuilder();
            sb.Append("{\"define_info\":{\"accept\":\"" + defineInfo .accept+ "\",\"accept_encoding\":\"" + defineInfo.accept_encoding + "\",\"host\":\"" + defineInfo .host+ "\",\"referer\":\"" + defineInfo.referer + "\",\"user_agent\":\"" + defineInfo.user_agent + "\",\"origin\":\"" + defineInfo .origin+ "\",\"ip_remote_ipv4\":\"" + defineInfo .ip_remote_ipv4+ "\",\"ip_remote_ipv6\":\"" + defineInfo.ip_remote_ipv6 + "\",\"time\":" + defineInfo.time+ " },\"data_info\":" + json_data_info + "}");
            return sb.ToString();
        }
        public string GetSignatureHeader(string partner_code, string accessToken, string action_code, string typ = "JWT", string alg = "HS256", string cty = "escs-api;v=1")
        {
            if (string.IsNullOrEmpty(typ) || string.IsNullOrEmpty(alg) || string.IsNullOrEmpty(cty))
            {
                throw new Exception("Thiếu thông tin header Video Call");
            }
            return @"{}".AddPropertyStringJson("typ", typ)
                        .AddPropertyStringJson("alg", alg)
                        .AddPropertyStringJson("cty", cty)
                        .AddPropertyStringJson("partner", partner_code)
                        .AddPropertyStringJson("token", accessToken)
                        .AddPropertyStringJson("cat", "private")
                        .AddPropertyStringJson("action", action_code);
        }
        public async Task<HttpResponseMessage> CallApi<T>(string url, BaseRequest<T> data, string actionCode = "ESCSAPI")
        {
            //string jsonBase = JsonConvert.SerializeObject(data);
            service.DefaultRequestHeaders.Add("eAuthToken", HttpConfiguration.AccessToken);
            service.DefaultRequestHeaders.Add("eAction", actionCode);
            string base64UrlEncodePayLoad = Utilities.Base64UrlEncode(JsonConvert.SerializeObject(data.data_info));
            var signatureData = Utilities.Sha256Hash(base64UrlEncodePayLoad + "." + HttpConfiguration.SecretKey);
            service.DefaultRequestHeaders.Add("eSignature", signatureData);
            //var httpContent = new StringContent(jsonBase, Encoding.UTF8, "application/json");
            return await service.PostAsJsonAsync(url, data);
        }
    }
    public class HttpConfiguration
    {
        public static string BaseUrl { get; set; }
        public static string AccessToken { get; set; }
        public static string SecretKey { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string PartnerCode { get; set; }
        public static string CatPartner { get; set; }
        public static int SessionTimeOut { get; set; }
        public static string Environment { get; set; }
        public static bool Secure { get; set; }
        public static string UrlFolderFile { get; set; }
    }
}
