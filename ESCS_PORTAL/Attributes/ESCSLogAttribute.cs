using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ESCS_PORTAL.COMMON.Contants;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Http;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ESCS_PORTAL.Attributes
{
    public class ESCSLogAttribute : ActionFilterAttribute
    {
        public bool ResultAsJsonString { get; set; } = true;
        public string filename { get; private set; }
        public LogInfo log { get; private set; }
        public ESCSLogAttribute(bool ResultAsJsonString = true)
        {
            this.ResultAsJsonString = ResultAsJsonString;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var routingValues = context.RouteData.Values;
                var currentArea = (string)routingValues["area"] ?? string.Empty;
                var currentController = (string)routingValues["controller"] ?? string.Empty;
                var currentAction = (string)routingValues["action"] ?? string.Empty;
                var time = DateTime.Now.ToString("yyyyMMddHHmmss");
                escs_nguoi_dung authen = null;
                if (context.HttpContext.Request.Cookies.ContainsKey(ESCSConstants.ESCS_TOKEN))
                {
                    var session = context.HttpContext.Request.Cookies[ESCSConstants.ESCS_TOKEN]?.ToString();
                    string json = Utilities.DecryptByKey(session, ESCS_PORTAL.COMMON.Http.AppSettings.KeyEryptData);
                    if (!string.IsNullOrEmpty(json))
                        authen = JsonConvert.DeserializeObject<escs_nguoi_dung>(json);
                }
                filename = time + "_" + ((!string.IsNullOrEmpty(currentArea) ? $"{currentArea}_" : string.Empty) +
                    $"{currentController}_{currentAction}_{Guid.NewGuid().ToString("N")}.json").ToLower();

                var request = context.HttpContext.Request;
                log = new LogInfo
                {
                    Url = ($"{AppSettings.AppDomain}/" + (!string.IsNullOrEmpty(currentArea) ? $"{currentArea}/" : string.Empty) +
                    $"{currentController}/{currentAction}").ToLower(),
                    Method = request.Method,
                    Time = Convert.ToInt64(time),
                    User = authen
                };

                if (request.Method != HttpMethods.Get)
                {
                    request.EnableBuffering();
                    request.Body.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(
                        request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false,
                        bufferSize: -1,
                        leaveOpen: true))
                    {
                        var body = reader.ReadToEnd();
                        if (this.ResultAsJsonString)
                            log.Request = body;
                        else
                            log.Request = JsonConvert.DeserializeObject(body);
                        request.Body.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            catch {
                log = null;
            }
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Task task = new Task(() =>
            {
               try
                {
                    if (log!=null)
                    {
                        if (context.Result.GetType() == typeof(Microsoft.AspNetCore.Mvc.OkObjectResult))
                        {
                            var result = ((Microsoft.AspNetCore.Mvc.OkObjectResult)context.Result).Value;
                            if (this.ResultAsJsonString)
                                log.Respone = JsonConvert.SerializeObject(result);
                            else
                                log.Respone = result;
                        }
                        WriteData(log, filename);
                    }
                }
                catch { }
            });
            task.Start();
            base.OnActionExecuted(context);
        }
        private void WriteData(LogInfo log, string filename)
        {
            string[] time = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
            string basefolder = Path.Combine(COMMON.Http.NetworkCredentials.Items.ElementAt(0).PathLocal);
            //if (!Directory.Exists(basefolder)) return;
            if (!Directory.Exists(basefolder)) basefolder=Path.Combine(Directory.GetCurrentDirectory(), "FILE_CAM_XOA");
                string currentpath = Path.Combine(basefolder, "LOG", time[0], time[1], time[2]);
            if (!Directory.Exists(currentpath)) Directory.CreateDirectory(currentpath);
            string filepath = Path.Combine(currentpath, filename);
            var json = JsonConvert.SerializeObject(log, Formatting.Indented);
            File.WriteAllText(filepath, json);
        }
    }
    public class LogInfo
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public escs_nguoi_dung User { get; set; }
        public long Time { get; set; }
        public dynamic Request { get; set; } = new object();
        public dynamic Respone { get; set; } = new object();
    }
}
