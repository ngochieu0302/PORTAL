using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Common
{
    public class TextCompare
    {
        public static double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            source = source.ToLower().Replace("công", "").Replace("sơn", "").Replace("thay", "").Replace("hàn", "").Replace("gò", "").Replace("căn", "").Replace("sửa", "");
            target = target.ToLower().Replace("công", "").Replace("sơn", "").Replace("thay", "").Replace("hàn", "").Replace("gò", "").Replace("chỉnh", "").Replace("sửa", "");
            var jw = new RatcliffObershelp();
            return jw.Similarity(source, target);
        }
        public static double CalculateSimilarityGara(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            source = source.ToLower().Replace("công", "").Replace("ty", "").Replace("cổ", "").Replace("phần", "").Replace("ô", "").Replace("tô", "").Replace("TNHH", "").Replace("CTY", "").Replace("CONG TY", "");
            target = target.ToLower().Replace("công", "").Replace("ty", "").Replace("cổ", "").Replace("phần", "").Replace("ô", "").Replace("tô", "").Replace("TNHH", "");
            var jw = new RatcliffObershelp();
            return jw.Similarity(source, target);
        }
    }
}
