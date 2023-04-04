using ExternalApi.ConfigurationModel;
using ExternalApi.Models;
using ExternalApi.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using SignalRPushService.Models;
using System.Net.Http.Headers;

namespace SignalRPushService.Controllers
{
    public class PushController : ControllerBase
    {
        private readonly ExretnalApiModel _exretnalApiModel;

        private readonly ISwtToken _swtTokenService;
        public PushController(ISwtToken swtTokenService, IOptionsMonitor<ExretnalApiModel> optionsMonitor)
        {
            _swtTokenService = swtTokenService;
            _exretnalApiModel = optionsMonitor.CurrentValue;
        }

        [Route("SignalRAPI/Push")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task SendPush([FromBody] Message message)
        {
            var token = _swtTokenService.JwtGenerator();

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(_exretnalApiModel.SignalRLink + "/signalr", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();

            await connection.StartAsync();
            await connection.InvokeAsync("Send",
                   message.Value, message.ToSessionID, message.Channel);

            await connection.StopAsync();
            await connection.DisposeAsync();
        }

        [Route("SignalRAPI/JoinGroup")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task JoinGroup([FromBody] GroupModel input)
        {
            var token = _swtTokenService.JwtGenerator();

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(_exretnalApiModel.SignalRLink + "/signalr", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();

            await connection.StartAsync();
            await connection.InvokeAsync("JoinToGroup",
                   input.GroupName, input.SwtToken);

            await connection.StopAsync();
            await connection.DisposeAsync();
        }

        [Route("SignalRAPI/SendPushToGroup")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task SendPushToGroup([FromBody] Message input)
        {
            var token = _swtTokenService.JwtGenerator();

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(_exretnalApiModel.SignalRLink + "/signalr", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();

            await connection.StartAsync();
            await connection.InvokeAsync("SendToGroup",
                   input.ToSessionID, input.Channel, input.Value);

            await connection.StopAsync();
            await connection.DisposeAsync();
        }
    }
}
