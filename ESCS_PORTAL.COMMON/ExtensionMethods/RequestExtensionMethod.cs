using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ESCS_PORTAL.COMMON.Auth;
using ESCS_PORTAL.COMMON.Caches.interfaces;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Http;
using ESCS_PORTAL.COMMON.Request;
using ESCS_PORTAL.COMMON.Response;
using Microsoft.AspNetCore.Http;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ESCS_PORTAL.COMMON.ExtensionMethods
{
    public static class RequestExtensionMethod
    {
        #region cũ
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";
        public const string SESSION_KEY = "ESCS_OPENID_SESSION";
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        public static dynamic GetDataRequest(this HttpRequest rq, escs_nguoi_dung nguoi_dung, object data = null)
        {
            if (data != null)
            {
                return GetDictionaryDataFromRequest(JsonConvert.SerializeObject(data));
            }
            dynamic objParam = new ExpandoObject();
            objParam.ma_doi_tac_nsd = nguoi_dung.ma_doi_tac;
            objParam.ma_chi_nhanh_nsd = nguoi_dung.ma_chi_nhanh;
            objParam.nsd = nguoi_dung.nsd;
            objParam.pas = nguoi_dung.pas;

            if (rq.Method.ToUpper() == "GET")
            {
                string requestData = rq.QueryString.Value;
                if (string.IsNullOrEmpty(requestData))
                {
                    return objParam;
                }
                requestData = requestData.Substring(1, requestData.Length - 1);
                string[] arr = requestData.Split('&');
                if (arr != null && arr.Count() > 0)
                {
                    foreach (var item in arr)
                    {
                        var arrKeyVal = item.Split('=');
                        if (arrKeyVal != null && arrKeyVal.Count() > 0)
                        {
                            var value = "";
                            if (arrKeyVal.Count() >= 2 && arrKeyVal[1] != null)
                            {
                                value = arrKeyVal[1];
                            }
                            try { AddProperty(objParam, arrKeyVal[0], value); } catch { };
                        }
                    }
                    return objParam;
                }
                return objParam;
            }
            if (rq.Method.ToUpper() == "POST")
            {
                string requestData = "";
                using (StreamReader reader = new StreamReader(rq.Body, Encoding.UTF8))
                {
                    requestData = reader.ReadToEnd();
                }
                if (!string.IsNullOrEmpty(requestData))
                {
                    requestData = HttpUtility.UrlDecode(requestData);
                }

                if (!string.IsNullOrEmpty(requestData))
                {
                    dynamic objData = JsonConvert.DeserializeObject(requestData);
                    objData.ma_doi_tac_nsd = nguoi_dung.ma_doi_tac;
                    objData.ma_chi_nhanh_nsd = nguoi_dung.ma_chi_nhanh;
                    objData.nsd = nguoi_dung.nsd;
                    objData.pas = nguoi_dung.pas;
                    return objData;
                }
                return objParam;
            }
            return objParam;
        }
        public static BaseResponse<T> Result<T>(this HttpResponseMessage res)
        {
            var json_serializer = new JavaScriptSerializer();
            var jsonString = res.Content.ReadAsStringAsync().Result;
            var routes_list = JsonConvert.DeserializeObject<BaseResponse<T>>(jsonString);
            return routes_list;
        }
        public static BaseResponse<T,O> Result<T,O>(this HttpResponseMessage res)
        {
            var json_serializer = new JavaScriptSerializer();
            var jsonString = res.Content.ReadAsStringAsync().Result;
            var routes_list = JsonConvert.DeserializeObject<BaseResponse<T,O>>(jsonString);
            return routes_list;
        }
        public static object Result(this HttpResponseMessage res)
        {
            var jsonString = res.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.FloatParseHandling = FloatParseHandling.Decimal;
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            var routes_list = JsonConvert.DeserializeObject(jsonString, settings);
            return routes_list;
        }
        public static async Task<object> GetRespone(this HttpRequest rq, string action, object data = null)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostAsync(action, auth.access_token, data, defineInfo);
            return rp.Result();
        }
        public static Dictionary<string, string> GetDictionaryDataFromRequest(string json)
        {
            var dicObj = DeserializeData(json);
            Dictionary<string, string> dString = dicObj.ToDictionary(k => k.Key, k => k.Value == null ? "" : k.Value.ToString());
            return dString;
        }
        public static IDictionary<string, object> DeserializeData(string data)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            return DeserializeData(values);
        }
        public static IDictionary<string, object> DeserializeData(IDictionary<string, object> data)
        {
            Dictionary<string, object> dicChildren = new Dictionary<string, object>();
            List<string> keyExist = new List<string>();
            foreach (var key in data.Keys.ToArray())
            {
                var value = data[key];
                if (value is JArray)
                {
                    keyExist.Add(key);
                    //IList<object> lstObject = DeserializeData(value as JArray);
                    var lstObject = (value as JArray).ToObject<List<object>>();
                    if (lstObject != null)
                    {
                        int i = 0;
                        foreach (var item in lstObject)
                        {
                            if (item is JObject)
                            {
                                var itemDic = (item as JObject).ToObject<Dictionary<String, Object>>();
                                foreach (var itemTmp in itemDic)
                                {
                                    dicChildren.Add(key + "[" + i + "]." + itemTmp.Key, itemTmp.Value);
                                }
                            }
                            else
                            {
                                dicChildren.Add(key + "[" + i + "]", item);
                            }
                            i++;
                        }
                    }
                }
            }
            foreach (var key in keyExist)
            {
                data.Remove(key);
            }
            if (dicChildren != null)
            {
                foreach (var item in dicChildren)
                {
                    data.Add(item);
                }
            }
            return data;
        }
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }
            return false;
        }
        #endregion;
        public static string GetDataRequestNew(this HttpRequest rq, escs_nguoi_dung nguoi_dung)
        {
            string requestData = "";
            if (rq.Method.ToUpper() == "POST")
            {
                requestData = "";
                StringBuilder sb = new StringBuilder();
                using (StreamReader reader = new StreamReader(rq.Body, Encoding.UTF8))
                {
                    requestData = HttpUtility.UrlDecode(reader.ReadToEnd());
                }
                if (string.IsNullOrEmpty(requestData))
                {
                    requestData = "{}";
                }
                requestData = requestData.AddPropertyStringJson("ma_doi_tac_nsd", nguoi_dung.ma_doi_tac)
                                         .AddPropertyStringJson("ma_chi_nhanh_nsd", nguoi_dung.ma_chi_nhanh)
                                         .AddPropertyStringJson("nsd", nguoi_dung.nsd)
                                         .AddPropertyStringJson("pas", nguoi_dung.pas);
                return requestData;
            }
            return "";
        }
        public static string GetDataRequestNewNoneSession(this HttpRequest rq)
        {
            string requestData = "";
            if (rq.Method.ToUpper() == "POST")
            {
                requestData = "";
                StringBuilder sb = new StringBuilder();
                using (StreamReader reader = new StreamReader(rq.Body, Encoding.UTF8))
                {
                    requestData = HttpUtility.UrlDecode(reader.ReadToEnd());
                }
                if (string.IsNullOrEmpty(requestData))
                {
                    requestData = "{}";
                }
                return requestData;
            }
            return "";
        }
        public static string GetDataRequestNoneLogin(this HttpRequest rq)
        {
            string requestData = "";
            if (rq.Method.ToUpper() == "POST")
            {
                requestData = "";
                StringBuilder sb = new StringBuilder();
                using (StreamReader reader = new StreamReader(rq.Body, Encoding.UTF8))
                {
                    requestData = HttpUtility.UrlDecode(reader.ReadToEnd());
                }
                if (string.IsNullOrEmpty(requestData))
                {
                    requestData = "{}";
                }
                return requestData;
            }
            return "";
        }
        public static string AddPropertyStringJson(this string json, string key, string value)
        {
            if (json != "{}")
            {
                json = json.Substring(0, json.Length - 1) + ",\"" + key + "\":\"" + value + "\"}";
            }
            else
            {
                json = json.Substring(0, json.Length - 1) + "\"" + key + "\":\"" + value + "\"}";
            }
            return json;
        }
        public static string AddPropertyStringJson(this string json, string key, decimal? value)
        {
            json = json.Substring(0, json.Length - 1) + ",\"" + key + "\":" + value + " }";
            return json;
        }
        public static dynamic GetFormDataRequest(this HttpRequest rq, escs_nguoi_dung nguoi_dung, out IFormFileCollection files, object data = null)
        {
            files = rq.Form.Files;
            if (data != null)
            {
                return GetDictionaryDataFromRequest(JsonConvert.SerializeObject(data));
            }
            dynamic objParam = new ExpandoObject();
            objParam.ma_doi_tac_nsd = nguoi_dung.ma_doi_tac;
            objParam.ma_chi_nhanh_nsd = nguoi_dung.ma_chi_nhanh;
            objParam.nsd = nguoi_dung.nsd;
            objParam.pas = nguoi_dung.pas;

            if (rq.Method.ToUpper() == "POST")
            {
                string requestData = "";
                if (!Utilities.IsMultipartContentType(rq.ContentType))
                {
                    using (StreamReader reader = new StreamReader(rq.Body, Encoding.UTF8))
                    {
                        requestData = reader.ReadToEnd();
                    }
                }
                else if (rq.Form != null)
                {
                    requestData = Utilities.FormCollectionToJson(rq.Form);
                }
                if (!string.IsNullOrEmpty(requestData))
                {
                    requestData = HttpUtility.UrlDecode(requestData);
                }

                if (!string.IsNullOrEmpty(requestData))
                {
                    dynamic objData = JsonConvert.DeserializeObject(requestData);
                    objData.ma_doi_tac_nsd = nguoi_dung.ma_doi_tac;
                    objData.ma_chi_nhanh_nsd = nguoi_dung.ma_chi_nhanh;
                    objData.nsd = nguoi_dung.nsd;
                    objData.pas = nguoi_dung.pas;
                    return objData;
                }
                return objParam;
            }

            return objParam;
        }
        public static dynamic GetFormDataRequestNoneLogin(this HttpRequest rq, out IFormFileCollection files, object data = null)
        {
            files = rq.Form.Files;
            if (data != null)
            {
                return GetDictionaryDataFromRequest(JsonConvert.SerializeObject(data));
            }
            dynamic objParam = new ExpandoObject();
            

            if (rq.Method.ToUpper() == "POST")
            {
                string requestData = "";
                if (!Utilities.IsMultipartContentType(rq.ContentType))
                {
                    using (StreamReader reader = new StreamReader(rq.Body, Encoding.UTF8))
                    {
                        requestData = reader.ReadToEnd();
                    }
                }
                else if (rq.Form != null)
                {
                    requestData = Utilities.FormCollectionToJson(rq.Form);
                }
                if (!string.IsNullOrEmpty(requestData))
                {
                    requestData = HttpUtility.UrlDecode(requestData);
                }

                if (!string.IsNullOrEmpty(requestData))
                {
                    dynamic objData = JsonConvert.DeserializeObject(requestData);
                    return objData;
                }
                return objParam;
            }

            return objParam;
        }
        public static async Task<BaseResponse<T>> GetRespone<T>(this HttpRequest rq, string action, object data = null)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostAsync(action, auth.access_token, data, defineInfo);
            return rp.Result<T>();
        }
        public static async Task<object> UploadFiles(this HttpRequest rq, string action, object data = null, IFormFileCollection files = null)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostFormDataAsync(action, auth.access_token, data, defineInfo, files);
            return rp.Result();
        }

        public static async Task<object> AttachFileEmail(this HttpRequest rq, string action, object data = null, IFormFileCollection files = null)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.AttachFileEmail(action, auth.access_token, data, defineInfo, files);
            return rp.Result();
        }

        public static async Task<BaseResponse<T>> UploadFiles<T>(this HttpRequest rq, string action, object data = null, IFormFileCollection files = null)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostFormDataAsync(action, auth.access_token, data, defineInfo, files);
            return rp.Result<T>();
        }
        public static async Task<object> PostDataAndFile(this HttpRequest rq, string url,  string action, string json,List<string> files, string type = "/api/esmartclaim/excute")
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostDataAndFileByPathAsync(url, action, auth.access_token, json, defineInfo, files);
            return rp.Result();
        }
        public static async Task<object> GetResponeNew(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/excute")
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
            if (type == "/api/esmartclaim/export-pdf" || type == "signature-file-pdf")
                return rp;
            return rp.Result();
        }
        public static async Task<object> GetResponeNewWithDefineInfo(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/excute", DefineInfo defineInfo = null)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            var rp = await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
            if (type == "/api/esmartclaim/export-pdf" || type == "signature-file-pdf")
                return rp;
            return rp.Result();
        }

        public static async Task<BaseResponse<T>> GetResponeNew<T>(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/excute")
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
            if (type == "/api/esmartclaim/export-pdf" || type == "signature-file-pdf")
                return null;
            return rp.Result<T>();
        }
        public static async Task<BaseResponse<T, O>> GetResponeNew<T, O>(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/excute")
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            var rp = await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
            if (type == "/api/esmartclaim/export-pdf" || type == "signature-file-pdf")
                return null;
            return rp.Result<T, O>();
        }
        public static async Task<BaseResponse<T>> GetResponeNewWithDefineInfo<T>(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/excute", DefineInfo defineInfo = null)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            var rp = await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
            if (type == "/api/esmartclaim/export-pdf" || type == "signature-file-pdf")
                return null;
            return rp.Result<T>();
        }
        public static async Task<HttpResponseMessage> GeneratePdfFile(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/export-pdf")
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            return await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
        }
        public static async Task<HttpResponseMessage> ExportExcel(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/export-bao-cao")
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            return await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
        }
        public static async Task<HttpResponseMessage> ExportBaoCao(this HttpRequest rq, string action, string json, string type = "/api/esmartclaim/export-bao-cao")
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            return await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, type);
        }
        public static async Task<HttpResponseMessage> GeneratePdfFileSignature(this HttpRequest rq, string action, string json)
        {
            var timeNow = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
            IHttpService _service = new HttpService();
            authentication auth = new authentication();
            if (HttpConfiguration.CatPartner == "PUBLIC")
            {
                auth = JsonConvert.DeserializeObject<authentication>(rq.HttpContext.Session.GetString(SESSION_KEY));
                if (timeNow >= Convert.ToInt64(auth.time_exprive.NumberToDateTime().Value.AddMinutes(-3).ToString("yyyyMMddHHmmss")))
                {
                    var resRefresh = await _service.RefreshToken(auth);
                    auth = resRefresh.Result<authentication>().data_info;
                    rq.HttpContext.Session.SetString(SESSION_KEY, JsonConvert.SerializeObject(auth));
                }
            }
            else
            {
                auth.access_token = HttpConfiguration.AccessToken;
            }
            DefineInfo defineInfo = new DefineInfo();
            defineInfo.accept = rq.Headers["Accept"].ToString();
            defineInfo.accept_encoding = rq.Headers["Accept-Encoding"].ToString();
            defineInfo.host = rq.Headers["Host"].ToString();
            defineInfo.user_agent = rq.Headers["User-Agent"].ToString();
            defineInfo.origin = rq.Headers["Origin"].ToString();
            defineInfo.referer = rq.Headers["Referer"].ToString();
            defineInfo.ip_remote_ipv4 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            defineInfo.ip_remote_ipv6 = rq.HttpContext.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
            return await _service.PostJsonAsync(action, auth.access_token, json, defineInfo, "signature-file-pdf");
        }
        /// <summary>
        /// Đang thực hiện
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="nguoi_dung"></param>
        /// <returns></returns>
        public static string GetDataExcelFile(this HttpRequest rq, escs_nguoi_dung nguoi_dung)
        {
            string requestData = "";
            if (rq.Method.ToUpper() == "POST")
            {
                requestData = Utilities.FormCollectionToJson(rq.Form);
                requestData = requestData.AddPropertyStringJson("ma_doi_tac_nsd", nguoi_dung.ma_doi_tac)
                                         .AddPropertyStringJson("ma_chi_nhanh_nsd", nguoi_dung.ma_chi_nhanh)
                                         .AddPropertyStringJson("nsd", nguoi_dung.nsd)
                                         .AddPropertyStringJson("pas", nguoi_dung.pas);
                return requestData;
            }
            return "";
        }
    }
}
