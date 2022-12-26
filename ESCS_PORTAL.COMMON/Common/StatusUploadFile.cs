using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Common
{
    public class StatusUploadFileConstant
    {
        public const string SUCCESS = "SUCCESS";
        public const string ERROR = "ERROR";
    }
    public class StatusUploadFile
    {
        public int index_file { get; set; }
        public string base_url { get; set; }
        public string path_file { get; set; }
        public string file_name { get; set; }
        public string extension_file { get; set; }
        public string status_upload { get; set; }
        public string error_message { get; set; }
        public StatusUploadFile(string base_url, string path_file,string file_name, string extension_file, int index_file, string status_upload = StatusUploadFileConstant.SUCCESS, string error_message = "")
        {
            this.index_file = index_file;
            this.path_file = path_file;
            this.file_name = file_name;
            this.extension_file = extension_file;
            this.status_upload = status_upload;
            this.error_message = error_message;
        }
    }
}
