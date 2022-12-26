using ESCS_PORTAL.COMMON.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Request
{
    public class BaseRequestEscs
    {
        public BaseRequestEscs()
        {
            
        }
        /// <summary>
        /// Thông tin định nghĩa request của client
        /// </summary>
        public DefineInfo define_info { get; set; }
        /// <summary>
        /// Thông tin định nghĩa đối tác
        /// </summary>
        //public UserInfo user_info { get; set; }
        /// <summary>
        /// Thông tin dữ liệu
        /// </summary>
        public dynamic data_info { get; set; }
        /// <summary>
        /// Chữ ký dữ liệu
        /// </summary>
        //public string signature { get; set; }
        /// <summary>
        /// Thông tin request và response
        /// </summary>
        public States states { get; set; }

        public BaseRequestEscs(dynamic _data_info)
        {
            data_info = _data_info;
        }
        
        public BaseRequestEscs(dynamic _data_info, DefineInfo _define_info = null)
        {
            if (_define_info == null)
            {
                _define_info = new DefineInfo();
            }
            define_info = _define_info;
            data_info = _data_info;
        }
        public BaseRequestEscs(dynamic _data_info, DefineInfo _define_info = null, States _state_info = null)
        {
            if (_define_info == null)
            {
                _define_info = new DefineInfo();
            }
            if (_state_info == null)
            {
                _state_info = new States();
            }
            define_info = _define_info;
            data_info = _data_info.ToDictionaryModel();
            states = _state_info;
        }
    }
}
