using ESCS_PORTAL.COMMON.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.Request
{
    public class BaseRequest
    {
        public BaseRequest()
        {
            data_info = new Dictionary<string, string>();
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
        public Dictionary<string, string> data_info { get; set; }
        /// <summary>
        /// Chữ ký dữ liệu
        /// </summary>
        //public string signature { get; set; }
        /// <summary>
        /// Thông tin request và response
        /// </summary>
        public States states { get; set; }
        //public BaseRequest()
        //{
        //    user_info = new UserInfo();
        //}
        public BaseRequest(DefineInfo _define_info)
        {
            define_info = _define_info;
            //user_info = new UserInfo();
        }
        public BaseRequest(Dictionary<string, string> _data_info)
        {
            data_info = _data_info;
            //user_info = new UserInfo();
        }
        public BaseRequest(object _data_info)
        {
            data_info = _data_info.ToDictionaryModel();
            //user_info = new UserInfo();
        }
        public BaseRequest(Dictionary<string, string> _data_info, DefineInfo _define_info = null)
        {
            if (_define_info == null)
            {
                _define_info = new DefineInfo();
            }
            define_info = _define_info;
            data_info = _data_info;
            //user_info = new UserInfo();
        }
        public BaseRequest(object _data_info, DefineInfo _define_info = null, States _state_info = null)
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
            //user_info = new UserInfo();
        }
    }
    public class BaseRequest<T>
    {
        public DefineInfo define_info { get; set; }
        public T data_info { get; set; }
        public States states { get; set; }
        public BaseRequest()
        {

        }
        public BaseRequest(DefineInfo _define_info)
        {
            define_info = _define_info;
        }
        public BaseRequest(T _data_info)
        {
            data_info = _data_info;
        }
        public BaseRequest(T _data_info, DefineInfo _define_info = null)
        {
            if (_define_info == null)
            {
                _define_info = new DefineInfo();
            }
            define_info = _define_info;
            data_info = _data_info;
        }
    }
}
