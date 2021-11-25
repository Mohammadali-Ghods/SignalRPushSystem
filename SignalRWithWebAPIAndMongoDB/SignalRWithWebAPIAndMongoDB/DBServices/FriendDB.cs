using MongoDB.Entities;
using SignalRWithWebAPIAndMongoDB.Entities;
using SignalRWithWebAPIAndMongoDB.HelperServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWithWebAPIAndMongoDB.DBServices
{
    public class FriendDB
    {
        Security _security;
        public FriendDB(Security security)
        {
            _security = security;
        }
        public virtual async Task<Friend> GetByIdAsync(string id)
        {
            return await DB.Find<Friend>().OneAsync(id);
        }
        public async Task<Friend> GetByUserID(string UserID)
        {
            return await DB.Find<Friend>().Match(x => x.UserID == UserID).ExecuteFirstAsync();
        }

        public async Task AddAsync(Friend Friend)
        {
            Friend.ID = Guid.NewGuid().ToString();
            await Friend.SaveAsync();
        }

        public async Task UpdateAsync(Friend Friend)
        {
            await DB.Update<Friend>()
             .Match(x => x.UserID == Friend.UserID)
             .ModifyExcept(x => new { x.ID }, Friend)
             .ExecuteAsync();
        }

        public async Task DeleteAsync(string UserId)
        {
            await DB.DeleteAsync<Friend>(UserId);
        }

        public async Task<IReadOnlyList<Friend>> GetAllAsync()
        {
            var ListOfUserPerGroup = await DB.Find<Friend>()
                     .ExecuteAsync();
            return ListOfUserPerGroup.AsReadOnly();
        }
    }
}
