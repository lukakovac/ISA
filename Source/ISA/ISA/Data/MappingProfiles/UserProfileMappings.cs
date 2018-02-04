using AutoMapper;
using ISA.DataAccess.Models;
using ISA.Models.ManageViewModels;

namespace ISA.Data.MappingProfiles
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfile, IndexViewModel>().ReverseMap();
            CreateMap<UserProfile, FriendViewModel>().ReverseMap();
        }
    }
}
