using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalApi.Models;

namespace ExternalApi.Interfaces
{
    public interface ICustomerApi
    {
        Task UpdateSignalR(string swttoken, string signalr);
        Task<SignalRModel> GetSession(string inputtoken);
    }
}
