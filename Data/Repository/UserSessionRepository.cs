using Data.Interfaces;
using Data.Models;
using Data.Repository.Base;
using MongoDB.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class UserSessionRepository : Repository<Session>, IUserSessionRepository
    {
        public async Task<Session> GetBySwtToken(string swttoken)
        {
            return await DB.Find<Session>().Match(x => x.SwtToken == swttoken).ExecuteFirstAsync();
        }
        public async Task<List<Session>> GetAll(string userid)
        {
            return await DB.Find<Session>().Match(x => x.UserID == userid).ExecuteAsync();
        }
    }
}
