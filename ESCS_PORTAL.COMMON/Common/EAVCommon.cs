using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESCS_PORTAL.COMMON.Common
{
    public class EAVCommon
    {
        public static IEnumerable<EAVModel> GetValueByKey(string json, string key)
        {
            JObject jobject = JObject.Parse(json);
            var model = jobject.SelectTokens(key).Select(n=> new EAVModel(){ attribute = n.Path, value = n.Value<string>() });
            return model;
        }
    }
}
