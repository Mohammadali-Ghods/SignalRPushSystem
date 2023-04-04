using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserSessionRepository:IRepository<Session>
    {
        Task<Session> GetBySwtToken(string swttoken);
        Task<List<Session>> GetAll(string userid);
    }
}
