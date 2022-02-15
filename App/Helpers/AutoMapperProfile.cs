using App.DTOs.Requests;
using App.Models;
using AutoMapper;

namespace App.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterRequestDTO, User>()
                .ForMember(dest =>
                    dest.DisplayName,
                    opt => opt.MapFrom(src => src.Name));
        }

    }
}
