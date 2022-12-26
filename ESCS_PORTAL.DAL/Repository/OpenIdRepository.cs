using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Request;
using ESCS_PORTAL.DAL.Repository.Oracle;
using ESCS_PORTAL.MODEL.OpenID.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.DAL.Repository
{
    public interface IOpenIdRepository
    {
        Task<object> GetPartnerWithConfig(BaseRequest model);
        Task<object> LogAccount(BaseRequest model);
        Task<object> AddConnection(BaseRequest model);
        Task<object> UpdateConnection(BaseRequest model);
        Task<object> GetConnection(BaseRequest model);
        Task<object> ChangePassApi(BaseRequest model);
        Task<object> DisConnection(BaseRequest model);
    }
    public class OpenIdRepository : IOpenIdRepository
    {
        public async Task<object> AddConnection(BaseRequest model)
        {
            string package = "PKG_SYS_CONNECTION";
            string storedname = "PSYS_CONNECTION_ADD";
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model.data_info);
            dynamic data = await service.ExcuteNoneQueryAsync(package + "." + storedname, param);
            return data;
        }
        public async Task<object> DisConnection(BaseRequest model)
        {
            string package = "PKG_SYS_CONNECTION";
            string storedname = "PSYS_CONNECTION_LOGOUT";
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model.data_info);
            dynamic data = await service.ExcuteNoneQueryAsync(package + "." + storedname, param);
            return data;
        }
        public async Task<object> UpdateConnection(BaseRequest model)
        {
            string package = "PKG_SYS_CONNECTION";
            string storedname = "PSYS_CONNECTION_EDIT";
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model.data_info);
            dynamic data = await service.ExcuteNoneQueryAsync(package + "." + storedname, param);
            return data;
        }
        public async Task<object> GetPartnerWithConfig(BaseRequest model)
        {
            string package = "PKG_SYS_CACHE";
            string storedname = "PSYS_PARTNER_CACHE";
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model.data_info);
            dynamic data = await service.ExcuteSingleAsync(package + "." + storedname, param);
            return Utilities.ToObjectJson(data);
        }
        public async Task<object> GetConnection(BaseRequest model)
        {
            string package = "PKG_SYS_CONNECTION";
            string storedname = "PSYS_CONNECTION_GET";
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model.data_info);
            dynamic data = await service.ExcuteSingleAsync(package + "." + storedname, param);
            return Utilities.ToObjectJson(data);
        }
        public async Task<object> LogAccount(BaseRequest model)
        {
            string package = "PKG_SYS_CACHE";
            string storedname = "PSYS_LOCK_ACCOUNT";
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model.data_info);
            dynamic data = await service.ExcuteNoneQueryAsync(package + "." + storedname, param);
            return data;
        }
        public async Task<object> ChangePassApi(BaseRequest model)
        {
            string package = "PKG_SYS_PARTNER_CONFIG";
            string storedname = "PSYS_PARTNER_CONFIG_CHANGEPASS";
            OracleRepository<dynamic> service = new OracleRepository<dynamic>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model.data_info);
            dynamic data = await service.ExcuteNoneQueryAsync(package + "." + storedname, param);
            return data;
        }
    }
}
