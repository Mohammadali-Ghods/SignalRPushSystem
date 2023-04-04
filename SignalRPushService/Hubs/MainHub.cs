using ExternalApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace SignalRPushService.Hubs
{
    [Authorize]
    public class MainHub : Hub
    {
        private readonly ICustomerApi _customerApi;
        private readonly IPanelUserApi _panelUserApi;
        public MainHub(IPanelUserApi panelUserApi, ICustomerApi customerApi)
        {
            _panelUserApi = panelUserApi;
            _customerApi = customerApi;
        }
        public override async Task OnConnectedAsync()
        {
            await UpdateSignalRToken(Context.ConnectionId);
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await UpdateSignalRToken("");
        }
        public async Task Send(string message, string signalrtoken, string channel)
        {
            await Clients.Client(signalrtoken).SendAsync(channel, message);
        }

        public async Task JoinToGroup(string groupname, string swttoken)
        {
            var session = await _customerApi.GetSession(swttoken);
            if (session == null) session = await _panelUserApi.GetSession(swttoken);

            if (session != null)
                await Groups.AddToGroupAsync(session.SignalR, groupname);
        }
        public async Task SendToGroup(string groupname, string channel, string message)
        {
            await Clients.Group(groupname)
                .SendAsync(channel, message);
        }
        private async Task UpdateSignalRToken(string signalrtoken)
        {
            if (Context.User.Identity.Name == "_signalrclient") return;
            var role = GetRoleFromJWTToken();
            if (role == "Customer")
                await _customerApi.UpdateSignalR(
                    Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    , signalrtoken);
            else
                await _panelUserApi.UpdateSignalR(
                   Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                   , signalrtoken);
        }
        private string GetRoleFromJWTToken()
        {
            string role = "";

            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                role = claims.ToList()
                    .Where(x => x.Type ==
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    .Select(x => x.Value).FirstOrDefault();
            }

            return role;
        }
    }
}
