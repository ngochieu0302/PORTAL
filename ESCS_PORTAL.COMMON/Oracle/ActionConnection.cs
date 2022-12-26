using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Oracle
{
    public class ActionConnection
    {
        public string actionapicode { get; set; }
        public string connection_name { get; set; }
        public string actionprocode { get; set; }
        public string schemadb { get; set; }
        public string db_code { get; set; }
        public string server_name { get; set; }
        public string port { get; set; }
        public string db_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string db_category { get; set; }
        public string token { get; set; }
        public string secret_key { get; set; }
        public string type_excute { get; set; }
        //Auto set
        public string connectionstring { get; set; }
        public string pkg_name { get; set; }
        public string stored_name { get; set; }
        public Nullable<int> use_sid { get; set; }
        public string service_name { get; set; }
        public string sid { get; set; }
        public string cache_endpoint { get; set; }

        //Thông tin partner
        public string partner_code { get; set; }
        public string env { get; set; }
        public string cat_partner { get; set; }

        //Thông tin cấu hình cache
        public string type_cache { get; set; }//loại cache
        public Nullable<int> max_rq_cache { get; set; }//số lượng request
        public Nullable<int> max_time_cache { get; set; }//giây
        public Nullable<int> time_live_cache { get; set; }//giây
        public string cache_server_ip { get; set; }//server ip
        public string cache_connection_name { get; set; }//connection name
        public string cache_password { get; set; }//mật khẩu
        public string cache_port { get; set; }//port server cache
        public string cache_db_name { get; set; }//db index
        public string action_key_cache { get; set; }// action key cache
        public string actions_clear_cache { get; set; }// danh sach action clear cache
        public string param_cache { get; set; }
        public void SetConnect()
        {
            if (this.use_sid == 1)
            {
                connectionstring = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SID={2})));User Id={3};Password={4};", server_name, port, db_name, username, password);
            }
            else
            {
                connectionstring = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};", server_name, port, db_name, username, password);
            }
            if (
                !string.IsNullOrEmpty(this.cache_server_ip) &&
                !string.IsNullOrEmpty(this.cache_port) &&
                !string.IsNullOrEmpty(this.cache_password)
                )
            {
                this.cache_endpoint = this.cache_server_ip + ":" + this.cache_port + ",password=" + this.cache_password;
            }

        }
    }
}
