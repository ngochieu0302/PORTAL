using ESCS_PORTAL.COMMON.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ESCS_PORTAL.COMMON.ExtensionMethods
{
    public static class ExtensionMethod
    {
        public static Dictionary<string, string> ToDictionaryModel<T>(this T model)
        {
            return model.GetType()
                 .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                      .ToDictionary(prop => prop.Name, prop => prop.GetValue(model, null) == null ? null : prop.GetValue(model, null).ToString());
        }
        public static DateTime? NumberToDateTime(this long? time)
        {
            if (time==null)
            {
                return null;
            }
            string strTime = time.ToString();
            int year = Convert.ToInt32(strTime.Substring(0, 4));
            int month = Convert.ToInt32(strTime.Substring(4, 2));
            int day = Convert.ToInt32(strTime.Substring(6, 2));
            int hh = Convert.ToInt32(strTime.Substring(8, 2));
            int mm = Convert.ToInt32(strTime.Substring(10, 2));
            int ss = Convert.ToInt32(strTime.Substring(12, 2));
            return new DateTime(year, month, day, hh, mm, ss);
        }
        public static T Clone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            if (source != null)
            {
                foreach (object obj in source)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source != null)
            {
                foreach (T obj in source)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
