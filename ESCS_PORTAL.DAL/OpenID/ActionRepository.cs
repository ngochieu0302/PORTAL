using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.DAL.Repository.Oracle;
using ESCS_PORTAL.MODEL.OpenID.ModelView;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.DAL.OpenID
{
    public interface IActionRepository
    {
        Task<PaginationGenneric<openid_sys_action>> GetPaging(openid_sys_action search);
        Task<int> Save(openid_sys_action model);
    }
    public class ActionRepository : IActionRepository
    {
        public async Task<PaginationGenneric<openid_sys_action>> GetPaging(openid_sys_action search)
        {
            string package = "PKG_BUS_ACTION";
            string storedname = "PSYS_ACTION_PAGING";
            OracleRepository<openid_sys_action> service = new OracleRepository<openid_sys_action>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, search);
            PaginationGenneric<openid_sys_action> dataPaging = new PaginationGenneric<openid_sys_action>();
            var data = await service.ExcuteManyAsync(package + "." + storedname, param);
            dataPaging.data = data;
            dataPaging.tong_so_dong = param.Get<OracleDecimal>("b_tong_so_dong").Value;
            return dataPaging;
        }
        public async Task<int> Save(openid_sys_action model)
        {
            string package = "PKG_BUS_ACTION";
            string storedname = "PSYS_ACTION_SAVE";
            OracleRepository<openid_sys_action> service = new OracleRepository<openid_sys_action>(OpenIDConfig.ConnectString);
            OracleDynamicParameters param = service.GetParamWithValueByQuery(OpenIDConfig.DbName, OpenIDConfig.Schema, package, storedname, model);
            var data = await service.ExcuteNoneQueryAsync(package + "." + storedname, param);
            return data;
        }
    }
}
