using AutoMapper;
using SignalRWithWebAPIAndMongoDB.Entities;
using SignalRWithWebAPIAndMongoDB.Models;

namespace SignalRWithWebAPIAndMongoDB.AutoMapper
{
    public class FriendsProfile : Profile
    {
        public FriendsProfile()
        {
            CreateMap<Friend, FriendViewModel>();
            //    CreateMap<RegistrationModel, User>();
            //    CreateMap<SyncDataModel, User>();
        }
    }
}
