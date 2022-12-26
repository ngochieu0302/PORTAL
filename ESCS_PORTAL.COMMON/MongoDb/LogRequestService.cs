using ESCS_PORTAL.COMMON.MongoDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESCS_PORTAL.COMMON.MongoDb
{
    public interface ILogRequestService<T> where T : class
    {
        Task<T> AddLogRequestAsync(T obj);
        Task<T> UpdateLogRequestAsync(string id, T obj);
        Task<bool> RemoveLogRequestAsync(string id);
        Task<T> GetByIdAsync(string id);
    }
    public class LogRequestService<T> : ILogRequestService<T> where T : class
    {
        private ILogRequestRepository<T> _logRequestRepository;
        public LogRequestService(ILogRequestRepository<T> logRequestRepository)
        {
            _logRequestRepository = logRequestRepository;
        }
        public Task<T> AddLogRequestAsync(T obj)
        {
            return _logRequestRepository.Add(obj);
        }
        public Task<T> UpdateLogRequestAsync(string id, T obj)
        {
            return _logRequestRepository.Update(id, obj);
        }
        public Task<bool> RemoveLogRequestAsync(string id)
        {
            return _logRequestRepository.Remove(id);
        }
        public Task<T> GetByIdAsync(string id)
        {
            return _logRequestRepository.GetById(id);
        }
    }
}
