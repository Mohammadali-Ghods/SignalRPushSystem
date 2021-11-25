using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRWithWebAPIAndMongoDB.DBServices;
using SignalRWithWebAPIAndMongoDB.Entities;
using SignalRWithWebAPIAndMongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRWithWebAPIAndMongoDB.Hubs
{
    [Authorize]
    public class FriendServiceHub : Hub
    {
        FriendDB _frienddb;
        private IMapper _mapper;
        public FriendServiceHub(FriendDB frienddb,
            IMapper mapper)
        {
            _frienddb = frienddb;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var fullname = ((ClaimsIdentity)Context.User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).FirstOrDefault();

            var friend = await _frienddb.GetByUserID(Context.User.Identity.Name);

            if (friend == null)
            {
                await _frienddb.AddAsync(new Entities.Friend()
                {
                    FullName = fullname,
                    UserID = Context.User.Identity.Name,
                    SignalRContextID = Context.ConnectionId,
                    UserStatus = Status.Online,
                    MyFriendsList = new List<Friend>()
                });
                await Clients.Client(Context.ConnectionId).SendAsync("ListOfFriend", null);
            }
            else
            {
                friend.UserStatus = Status.Online;
                friend.FullName = fullname;
                friend.SignalRContextID = Context.ConnectionId;
                await _frienddb.UpdateAsync(friend);
                await Clients.Client(Context.ConnectionId).SendAsync("ListOfFriend", _mapper.Map<FriendViewModel>(friend.MyFriendsList));
            }
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var friend = await _frienddb.GetByUserID(Context.User.Identity.Name);
            friend.UserStatus = Status.Offline;
            await _frienddb.UpdateAsync(friend);
        }

    }
}
