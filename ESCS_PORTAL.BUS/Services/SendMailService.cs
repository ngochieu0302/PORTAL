using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Http;
using ESCS_PORTAL.COMMON.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ESCS_PORTAL.COMMON.ExtensionMethods;

namespace ESCS_PORTAL.BUS.Services
{
    public interface ISendMailService
    {
    }
    public class SendMailService: ISendMailService
    {
        public SendMailService()
        {

        }
        private string GetContentTemplate(string ma_mau_email, string action_code)
        {
            string pathFile = "";
            NetworkCredentialItem network = NetworkCredentials.GetItem("ESCS_PATH_FILE");
            pathFile = Path.Combine(network.PathLocal, "FILE_CAM_XOA", pathFile);
            if (!System.IO.File.Exists(pathFile))
            {
                return "";
            }
            string template_mail  = System.IO.File.ReadAllText(pathFile);
            return template_mail;
        }
    }
}
