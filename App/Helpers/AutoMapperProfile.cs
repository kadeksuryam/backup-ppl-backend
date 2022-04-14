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

            CreateMap<User, GetDisplayNameResponseDTO>();

            CreateMap<User, GetBankTopUpRequestResponseDTO.UserDTO>();
            CreateMap<Bank, GetBankTopUpRequestResponseDTO.BankDTO>();

            CreateMap<Voucher, GetVoucherResponseDTO>();

            CreateMap<CreateTransactionRequestDTO, TransactionHistory>();
            CreateMap<TransactionHistory, CreateTransactionResponseDTO>();
            CreateMap<User, CreateTransactionResponseDTO.UserDTO>()
                .ForMember(dest =>
                    dest.PreviousBalance, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.CurrentBalance, opt => opt.Ignore());

            CreateMap<User, GetTransactionHistoryResponseDTO.UserDTO>();
            CreateMap<User, GetTopUpHistoryResponseDTO.UserDTO>();
            CreateMap<Voucher, GetTopUpHistoryResponseDTO.VoucherDTO>();
            CreateMap<Bank, GetTopUpHistoryResponseDTO.BankDTO>();
        }

    }
}
