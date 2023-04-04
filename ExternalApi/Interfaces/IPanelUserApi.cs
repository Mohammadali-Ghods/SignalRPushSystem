using ExternalApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalApi.Interfaces
{
    public interface IPanelUserApi
    {
        Task UpdateSignalR(string swttoken,string signalr);
        Task<SignalRModel> GetSession(string inputtoken);
    }
}
