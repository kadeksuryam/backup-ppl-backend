using App.DTOs.Requests;
using App.DTOs.Responses;
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

            CreateMap<User, LoginResponseDTO>();

            CreateMap<User, GetProfileResponseDTO>()
                .ForMember(dest =>
                    dest.Level, opt => opt.Ignore());

            CreateMap<User, GetBankTopUpRequestResponseDTO.UserDTO>();
            CreateMap<Bank, GetBankTopUpRequestResponseDTO.BankDTO>();

            CreateMap<Voucher, GetVoucherResponseDTO>();
        }

    }
}
