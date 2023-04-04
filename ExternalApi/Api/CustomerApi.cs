using ExternalApi.ConfigurationModel;
using ExternalApi.HttpModule;
using ExternalApi.Interfaces;
using ExternalApi.Models;
using ExternalApi.TokenService;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExternalApi.Api
{
    public class CustomerApi : ICustomerApi
    {
        #region Fields
        private readonly ExretnalApiModel _exretnalApiModel;
        private readonly ISwtToken _swtToken;
        #endregion

        #region Ctor
        public CustomerApi(IOptionsMonitor<ExretnalApiModel> optionsMonitor
            , ISwtToken swtToken)
        {
            _exretnalApiModel = optionsMonitor.CurrentValue;
            _swtToken = swtToken;
        }
        #endregion

        public async Task UpdateSignalR(string swttoken, string signalr)
        {
            var token = "Bearer " + _swtToken.JwtGenerator();

            await BaseHttp.Post<SignalRModel>
                (
                new Dictionary<string, string>() { { "Authorization", token } }
                , _exretnalApiModel.PaymentMicroserviceUrl + "/updatesignalrtoken", new SignalRModel()
                {
                    SignalR = signalr,
                    SwtToken = swttoken
                });
        }
        public async Task<SignalRModel> GetSession(string inputtoken)
        {
            var swttoken = "Bearer " + _swtToken.JwtGenerator();

            var Model = await BaseHttp.Get<SignalRModel>
                (
                new Dictionary<string, string>() { { "Authorization", swttoken } }
                , _exretnalApiModel.PaymentMicroserviceUrl, "/customersession/" + inputtoken);

            return Model;
        }
    }
}
