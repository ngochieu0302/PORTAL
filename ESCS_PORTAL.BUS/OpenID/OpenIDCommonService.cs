using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.DAL.OpenID;
using ESCS_PORTAL.MODEL.OpenID.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.BUS.OpenID
{
    public interface IOpenIDCommonService
    {
        Task<BaseResponse<openid_category_result>> GetCategoryOpenId(string envcode);
    }
    public class OpenIDCommonService : IOpenIDCommonService
    {
        private readonly IOpenIDCommonRepository _openIDCommonRepository;
        public OpenIDCommonService(IOpenIDCommonRepository openIDCommonRepository)
        {
            _openIDCommonRepository = openIDCommonRepository;
        }
        public async Task<BaseResponse<openid_category_result>> GetCategoryOpenId(string envcode)
        {
            return await  _openIDCommonRepository.GetCategoryOpenId(envcode);
        }
    }
}
