using ESCS_PORTAL.COMMON.Response;
using ESCS_PORTAL.DAL.OpenID;
using ESCS_PORTAL.MODEL.OpenID.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.BUS.OpenID
{
    public interface IActionService
    {
        Task<PaginationGenneric<openid_sys_action>> GetPaging(openid_sys_action search);
        Task<int> Save(openid_sys_action model);
    }
    public class ActionService : IActionService
    {
        private readonly IActionRepository _actionRepository;
        public ActionService(IActionRepository actionRepository)
        {
            _actionRepository = actionRepository;
        }
        public async Task<PaginationGenneric<openid_sys_action>> GetPaging(openid_sys_action search)
        {
            return await _actionRepository.GetPaging(search);
        }
        public async Task<int> Save(openid_sys_action model)
        {
            return await _actionRepository.Save(model);
        }
    }
}
