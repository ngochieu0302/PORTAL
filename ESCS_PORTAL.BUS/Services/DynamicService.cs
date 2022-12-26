using ESCS_PORTAL.COMMON.Caches.interfaces;
using ESCS_PORTAL.COMMON.Common;
using ESCS_PORTAL.COMMON.ExceptionHandlers;
using ESCS_PORTAL.COMMON.Oracle;
using ESCS_PORTAL.COMMON.Request;
using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.BUS.Services
{
    public interface IDynamicService
    {
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
        void RemoveCacheParam();
        #endregion
        Task<object> ExcuteAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null);
        Task<dynamic> ExcuteDynamicAsync(BaseRequest model, HeaderRequest partner, Dictionary<string, bool> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null);
        ActionConnection GetConnection(HeaderRequest partner);
        Task ClearCacheActions(HeaderRequest partner, ICacheServer _cacheServer, string actions, BaseRequest rq);
    }
    public class DynamicService : IDynamicService
    {
        private readonly IDynamicRepository _dynamicRepository;
        public DynamicService(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }
        public object Get(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return _dynamicRepository.Get(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<object> GetAsync(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return await _dynamicRepository.GetAsync(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public object GetScalar(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return _dynamicRepository.GetScalar(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<object> GetScalarAsync(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return await _dynamicRepository.GetScalarAsync(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public object GetList(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return _dynamicRepository.GetList(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<object> GetListAsync(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return await _dynamicRepository.GetListAsync(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public object GetMultiple(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return _dynamicRepository.GetMultiple(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<object> GetMultipleAsync(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return await _dynamicRepository.GetMultipleAsync(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public int PostData(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return _dynamicRepository.PostData(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<int> PostDataAsync(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return await _dynamicRepository.PostDataAsync(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<Pagination> GetPagingAsync(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return await _dynamicRepository.GetPagingAsync(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public Pagination GetPaging(BaseRequest model, HeaderRequest partner)
        {
            try
            {
                return _dynamicRepository.GetPaging(model, partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public void RemoveCacheParam()
        {
            OracleRepositoryConstant.listParam = null;
        }
        public ActionConnection GetConnection(HeaderRequest partner)
        {
            try
            {
                return _dynamicRepository.GetConnection(partner);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<int> PostDataMultipleAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs)
        {
            try
            {
                return await _dynamicRepository.PostDataMultipleAsync(model, partner, prefixs);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<object> PostDataMultipleScalarAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs)
        {
            try
            {
                return await _dynamicRepository.PostDataMultipleScalarAsync(model, partner, prefixs);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<object> ExcuteAsync(BaseRequest model, HeaderRequest partner, IEnumerable<string> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null)
        {
            try
            {
                return await _dynamicRepository.ExcuteAsync(model, partner, prefixs, actionOutPutValue);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task<dynamic> ExcuteDynamicAsync(BaseRequest model, HeaderRequest partner, Dictionary<string, bool> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null)
        {
            try
            {
                return await _dynamicRepository.ExcuteDynamicAsync(model, partner, prefixs, actionOutPutValue);
            }
            catch (Exception ex)
            {
                throw new OracleDbException(ex);
            }
        }
        public async Task ClearCacheActions(HeaderRequest partner, ICacheServer _cacheServer, string actions, BaseRequest rq = null)
        {
            if (!string.IsNullOrEmpty(actions))
            {
                foreach (var action in actions.Split(","))
                {
                    try
                    {
                        await Task.Run(() => {
                            HeaderRequest partner_new = new HeaderRequest();
                            partner_new.action = action;
                            //partner_new.envcode = partner.envcode;
                            partner_new.token = partner.token;
                            partner_new.partner_code = partner.partner_code;
                            var conn = GetConnection(partner_new);
                            string keyCache = Utilities.GetKeyCache(conn, null);
                            if (!string.IsNullOrEmpty(conn.cache_connection_name) && !string.IsNullOrEmpty(conn.cache_endpoint))
                            {
                                _cacheServer.RemoveKeyCacheByPattern(conn.cache_endpoint, keyCache, Convert.ToInt32(conn.cache_db_name));
                            }
                        });
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
