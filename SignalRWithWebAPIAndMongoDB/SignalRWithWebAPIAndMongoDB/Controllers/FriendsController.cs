using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SignalRWithWebAPIAndMongoDB.DBServices;
using SignalRWithWebAPIAndMongoDB.Entities;
using SignalRWithWebAPIAndMongoDB.HelperServices;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SignalRWithWebAPIAndMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        FriendDB _frienddb;
        IConfiguration _configuration;
        private IMapper _mapper;
        Security _security;
        public FriendsController(FriendDB frienddb, IConfiguration configuration,
            IMapper mapper, Security security)
        {
            _frienddb = frienddb;
            _configuration = configuration;
            _mapper = mapper;
            _security = security;
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings")["Secret"]);
            return Ok(_security.GenerateToken(secret, "ghods", "admin"));
        }
    }
}
