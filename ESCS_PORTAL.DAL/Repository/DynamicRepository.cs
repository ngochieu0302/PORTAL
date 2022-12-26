using ESCS_PORTAL.COMMON.Caches;
using ESCS_PORTAL.COMMON.Caches.interfaces;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Contants;
using ESCS_PORTAL.COMMON.Oracle;
using ESCS_PORTAL.COMMON.Request;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.DAL.Repository.Oracle;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.DAL.Repository
{
    public interface IDynamicRepository
    {
        Task<object> ExcuteAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null);
        Task<dynamic> ExcuteDynamicAsync(BaseRequest model, HeaderRequest partner, Dictionary<string, bool> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null);
        #region Thành phần cũ
        Pagination GetPaging(BaseRequest model, HeaderRequest partner);
        Task<Pagination> GetPagingAsync(BaseRequest model, HeaderRequest partner);
        object GetScalar(BaseRequest model, HeaderRequest partner);
        Task<object> GetScalarAsync(BaseRequest model, HeaderRequest partner);
        object Get(BaseRequest model, HeaderRequest partner);
        Task<object> GetAsync(BaseRequest model, HeaderRequest partner);
        object GetList(BaseRequest model, HeaderRequest partner);
        Task<object> GetListAsync(BaseRequest model, HeaderRequest partner);
        object GetMultiple(BaseRequest model, HeaderRequest partner);
        Task<object> GetMultipleAsync(BaseRequest model, HeaderRequest partner);
        int PostData(BaseRequest model, HeaderRequest partner);
        Task<int> PostDataAsync(BaseRequest model, HeaderRequest partner);
        Task<int> PostDataMultipleAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs);
        Task<object> PostDataMultipleScalarAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs);
        ActionConnection GetConnection(HeaderRequest model);
        #endregion
    }
    public class DynamicRepository : IDynamicRepository
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly ICacheServer _cacheServer;
        public  DynamicRepository(ICacheServer cacheServer, IMemoryCacheManager cacheManager)
        {
            _cacheServer = cacheServer;
            _cacheManager = cacheManager;
        }
        public async Task<object> ExcuteAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (prefixs != null && prefixs.Count() > 0)
            {
                foreach (var prefix in prefixs)
                {
                    param.MapArrayValue(prefix, model.data_info);
                }
            }
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            Dictionary<string, object> outPutData = new Dictionary<string, object>();
            dynamic data = null;

            switch (conn.type_excute.ToUpper())
            {
                case "RETURN_NONE":
                    data = await service.ExcuteNoneQueryAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return data;
                case "RETURN_SCALAR":
                    data = await service.ExcuteScalarAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return data;
                case "RETURN_SINGLE":
                    data = await service.ExcuteSingleAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return Utilities.ToObjectJson(data);
                case "RETURN_PAGING":
                    Pagination dataPaging = new Pagination();
                    data = await service.ExcuteManyAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    dataPaging.data = Utilities.ToListObjectJson(data);
                    dataPaging.tong_so_dong = param.Get<OracleDecimal>("b_tong_so_dong").Value;
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return dataPaging;
                case "RETURN_LIST":
                    data = await service.ExcuteManyAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return Utilities.ToListObjectJson(data);
                case "RETURN_MULTIPLE":
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    await service.ExcuteMultipleAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param, grid => {
                        foreach (var p in param.parameters)
                        {
                            if (p.Key.ToLower().StartsWith("cur_"))
                            {
                                dic.Add(p.Key.Substring(4, p.Key.Length - 4), Utilities.ToObjectJson(grid.Read<object>().FirstOrDefault()));
                            }
                            if (p.Key.ToLower().StartsWith("curs_"))
                            {
                                dic.Add(p.Key.Substring(5, p.Key.Length - 5), Utilities.ToListObjectJson(grid.Read<object>().ToList()));
                            }
                        }
                    });
                    dynamic res = dic;
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return res;
                default:
                    break;
            }
            return null;
        }
        public async Task<dynamic> ExcuteDynamicAsync(BaseRequest model, HeaderRequest partner, Dictionary<string, bool> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (prefixs != null && prefixs.Count() > 0)
            {
                foreach (var prefix in prefixs)
                {
                    param.MapArrayValue(prefix.Key, model.data_info,prefix.Value);
                }
            }
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            Dictionary<string, object> outPutData = new Dictionary<string, object>();
            dynamic data = null;

            switch (conn.type_excute.ToUpper())
            {
                case "RETURN_NONE":
                    data = await service.ExcuteNoneQueryAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return data;
                case "RETURN_SCALAR":
                    data = await service.ExcuteScalarAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return data;
                case "RETURN_SINGLE":
                    data = await service.ExcuteSingleAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return data;
                case "RETURN_PAGING":
                    Pagination dataPaging = new Pagination();
                    data = await service.ExcuteManyAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    dataPaging.data = data;
                    dataPaging.tong_so_dong = param.Get<OracleDecimal>("b_tong_so_dong").Value;
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return dataPaging;
                case "RETURN_LIST":
                    data = await service.ExcuteManyAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return data;
                case "RETURN_MULTIPLE":
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    await service.ExcuteMultipleAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param, grid => {
                        foreach (var p in param.parameters)
                        {
                            if (p.Key.ToLower().StartsWith("cur_"))
                            {
                                dic.Add(p.Key.Substring(4, p.Key.Length - 4), Utilities.ToObjectJson(grid.Read<object>().FirstOrDefault()));
                            }
                            if (p.Key.ToLower().StartsWith("curs_"))
                            {
                                dic.Add(p.Key.Substring(5, p.Key.Length - 5), Utilities.ToListObjectJson(grid.Read<object>().ToList()));
                            }
                        }
                    });
                    dynamic res = dic;
                    outPutData = GetOutPutValue(param);
                    if (actionOutPutValue != null)
                        actionOutPutValue(outPutData);
                    return res;
                default:
                    break;
            }
            return null;
        }
        private Dictionary<string, object> GetOutPutValue(OracleDynamicParameters param)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (param != null && param.parameters != null && param.parameters.Values != null)
            {
                var paramInfos = param.parameters.Values.Where(n => n.ParameterDirection != ParameterDirection.Input && n.DbType != OracleDbType.RefCursor).ToList();
                if (paramInfos != null)
                {
                    foreach (var paramInfo in paramInfos)
                    {
                        var propertyName = paramInfo.Name.ToLower().StartsWith("b_") ? paramInfo.Name.ToLower().Substring(2, paramInfo.Name.Length - 2) : paramInfo.Name.ToLower();
                        switch (paramInfo.DbType)
                        {
                            case OracleDbType.Decimal:
                                res.Add(propertyName, param.Get<OracleDecimal>(paramInfo.Name).Value);
                                break;
                            case OracleDbType.NChar:
                            case OracleDbType.NVarchar2:
                            case OracleDbType.Varchar2:
                            case OracleDbType.Char:
                                res.Add(propertyName, param.Get<OracleString>(paramInfo.Name).Value);
                                break;
                            case OracleDbType.Clob:
                            case OracleDbType.NClob:
                                res.Add(propertyName, param.Get<OracleClob>(paramInfo.Name).Value);
                                break;
                            case OracleDbType.Date:
                                res.Add(propertyName, param.Get<OracleDate>(paramInfo.Name).Value);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return res;
        }
        #region Thành phần cũ
        public object Get(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = service.ExcuteSingle(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return Utilities.ToObjectJson(data);
        }
        public async Task<object> GetAsync(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = await service.ExcuteSingleAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return Utilities.ToObjectJson(data);
        }
        public object GetScalar(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = service.ExcuteScalarAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return data;
        }
        public async Task<object> GetScalarAsync(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = await service.ExcuteScalarAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return data;
        }
        public object GetList(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = service.ExcuteMany(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return Utilities.ToListObjectJson(data);
        }
        public async Task<object> GetListAsync(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = await service.ExcuteManyAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return Utilities.ToListObjectJson(data);
        }
        public object GetMultiple(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (param != null && param.parameters.Count() > 0)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(conn.pkg_name))
                {
                    conn.pkg_name = "." + conn.pkg_name;
                }
                service.ExcuteMultiple(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param, grid => {
                    foreach (var p in param.parameters)
                    {
                        if (p.Key.ToLower().StartsWith("cur_"))
                        {
                            dic.Add(p.Key.Substring(4, p.Key.Length - 4), Utilities.ToObjectJson(grid.Read<object>().FirstOrDefault()));
                        }
                        if (p.Key.ToLower().StartsWith("curs_"))
                        {
                            dic.Add(p.Key.Substring(5, p.Key.Length - 5), Utilities.ToListObjectJson(grid.Read<object>().ToList()));
                        }
                    }
                });
                dynamic res = dic;
                return res;
            }
            return null;
        }
        public async Task<object> GetMultipleAsync(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (param != null && param.parameters.Count() > 0)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(conn.pkg_name))
                {
                    conn.pkg_name = "." + conn.pkg_name;
                }
                await service.ExcuteMultipleAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param, grid => {
                    foreach (var p in param.parameters)
                    {
                        if (p.Key.ToLower().StartsWith("cur_"))
                        {
                            dic.Add(p.Key.Substring(4, p.Key.Length - 4), Utilities.ToObjectJson(grid.Read<object>().FirstOrDefault()));
                        }
                        if (p.Key.ToLower().StartsWith("curs_"))
                        {
                            dic.Add(p.Key.Substring(5, p.Key.Length - 5), Utilities.ToListObjectJson(grid.Read<object>().ToList()));
                        }
                    }
                });
                dynamic res = dic;
                return res;
            }
            return null;
        }
        public int PostData(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = service.ExcuteNoneQuery(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return data;
        }
        public async Task<int> PostDataAsync(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = await service.ExcuteNoneQueryAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return data;
        }
        public async Task<Pagination> GetPagingAsync(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            Pagination dataPaging = new Pagination();
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = await service.ExcuteManyAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            dataPaging.data = Utilities.ToListObjectJson(data);
            dataPaging.tong_so_dong = param.Get<OracleDecimal>("b_tong_so_dong").Value;
            return dataPaging;
        }
        public Pagination GetPaging(BaseRequest model, HeaderRequest partner)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            Pagination dataPaging = new Pagination();
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = service.ExcuteMany(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            dataPaging.data = Utilities.ToListObjectJson(data);
            dataPaging.tong_so_dong = param.Get<OracleDecimal>("b_tong_so_dong").Value;
            return dataPaging;
        }
        public ActionConnection GetConnection(HeaderRequest partner)
        {
            string keyCache = CachePrefixKeyConstants.GetKeyCacheAction(partner.partner_code.Trim(), partner.envcode.Trim(), partner.action.Trim());
            string json = _cacheServer.Get<string>(RedisCacheMaster.ConnectionName, RedisCacheMaster.Endpoint, keyCache, RedisCacheMaster.DatabaseIndex);
            if (string.IsNullOrEmpty(json))
            {
                OracleRepository<ActionConnection> service = new OracleRepository<ActionConnection>(OpenIDConfig.ConnectString);
                OracleDynamicParameters param = new OracleDynamicParameters();
                param.Add("b_action", partner.action.Trim(), OracleDbType.Varchar2, ParameterDirection.Input);
                param.Add("b_partner_code", partner.partner_code.Trim(), OracleDbType.Varchar2, ParameterDirection.Input);
                param.Add("b_envcode", partner.envcode.Trim(), OracleDbType.Varchar2, ParameterDirection.Input);
                param.Add("cur_connect", null, OracleDbType.RefCursor, ParameterDirection.Output);
                ActionConnection conn = service.ExcuteSingle(OpenIDConfig.Schema + ".PKG_SYS_COMMON.PSP_GET_CONNECT_OPENID", param);
                if (conn == null)
                {
                    return null;
                }
                conn.SetConnect();
                _cacheServer.Set<string>(RedisCacheMaster.ConnectionName, RedisCacheMaster.Endpoint, keyCache,JsonConvert.SerializeObject(conn), DateTime.Now.AddDays(30) - DateTime.Now, RedisCacheMaster.DatabaseIndex);
                return conn;
            }
            return JsonConvert.DeserializeObject<ActionConnection>(json);
        }
        public async Task<int> PostDataMultipleAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");

            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (prefixs != null && prefixs.Count() > 0)
            {
                foreach (var prefix in prefixs)
                {
                    param.MapArrayValue(prefix, model.data_info);
                }
            }
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = await service.ExcuteNoneQueryAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return data;
        }
        public async Task<object> PostDataMultipleScalarAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs)
        {
            var conn = GetConnection(partner);
            if (conn == null)
                throw new Exception("Thông tin kết nối đối tác không hợp lệ");

            OracleRepository<dynamic> service = new OracleRepository<dynamic>(conn.connectionstring);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(conn.db_name, conn.schemadb, conn.pkg_name, conn.stored_name, model.data_info);
            if (prefixs != null && prefixs.Count() > 0)
            {
                foreach (var prefix in prefixs)
                {
                    param.MapArrayValue(prefix, model.data_info);
                }
            }
            if (!string.IsNullOrEmpty(conn.pkg_name))
            {
                conn.pkg_name = "." + conn.pkg_name;
            }
            var data = await service.ExcuteScalarAsync(conn.schemadb + conn.pkg_name + "." + conn.stored_name, param);
            return data;
        }
        #endregion
    }
}
