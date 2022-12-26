using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.ESCSStoredProcedures;
using ESCS_PORTAL.COMMON.ExtensionMethods;
using ESCS_PORTAL.COMMON.Http;
using ESCS_PORTAL.COMMON.Request;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.MODEL.ESCS;
using ESCS_PORTAL.MODEL.ESCS.ModelView;
using ESCS_PORTAL.MODEL.ESCS_PORTAL.ModelView;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESCS_PORTAL.Common
{
    public class EscsUtils
    {
        public static IRazorEngineService _service = null;
        public static void CreateConfigRazor()
        {
            TemplateServiceConfiguration config = new TemplateServiceConfiguration();
            config.CachingProvider = new RazorEngine.Templating.DefaultCachingProvider();
            if (_service == null)
                _service = RazorEngineService.Create(config);
        }

        private static Dictionary<string, IEnumerable<escs_menu>> user_menus = new Dictionary<string, IEnumerable<escs_menu>>();
        public static List<ht_cai_dat> cai_dat = null;
        public static void SaveUserMenu(string user, IEnumerable<escs_menu> menu)
        {
            if (!user_menus.ContainsKey(user))
            {
                user_menus.Add(user, menu);
            }
            else
            {
                user_menus[user] = menu;
            }
        }
        public static void RemoveMenu(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return;
            }
            if (user_menus.ContainsKey(user))
            {
                user_menus.Remove(user);
            }
        }
        public static IEnumerable<escs_menu> GetMenu(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return new List<escs_menu>();
            }
            if (user_menus.ContainsKey(user))
            {
                return user_menus[user];
            }
            return new List<escs_menu>();
        }
    }
}
