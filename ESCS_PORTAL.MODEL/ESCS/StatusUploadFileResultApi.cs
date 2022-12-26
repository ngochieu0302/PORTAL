using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.MODEL.ESCS
{
    public class StatusUploadFileResultConstant
    {
        public const string SUCCESS = "SUCCESS";
        public const string ERROR = "ERROR";
    }
    public class StatusUploadFileResultApi
    {
        public int index_file { get; set; }
        public string base_url { get; set; }
        public string path_file { get; set; }
        public string file_name { get; set; }
        public string file_name_new { get; set; }
        public string extension_file { get; set; }
        public string status_upload { get; set; }
        public string error_message { get; set; }
        public string thumnail_base64 { get; set; }
        public byte[] file { get; set; }


        public string ma_doi_tac { get; set; }
        public string so_id { get; set; }
        public string bt { get; set; }
        public string nhom_anh { get; set; }

        public StatusUploadFileResultApi(string base_url, string path_file, string file_name, string file_name_new, string nhom_anh, string extension_file, int index_file, string status_upload = StatusUploadFileResultConstant.SUCCESS, string error_message = "", byte[] file = null)
        {
            this.file = file;
            this.index_file = index_file;
            this.path_file = path_file;
            this.file_name = file_name;
            this.extension_file = extension_file;
            this.status_upload = status_upload;
            this.error_message = error_message;
            this.file_name_new = file_name_new;
            this.nhom_anh = nhom_anh;
        }
        public StatusUploadFileResultApi()
        {

        }
    }
}
