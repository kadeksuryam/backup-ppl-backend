using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Models;
using AutoMapper;
using System.Globalization;

namespace App.Helpers
{
    public class AutoMapperProfile : Profile
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

            CreateMap<BankTopUpRequestDTO, BankTopUpRequest>()
                .ForMember(dest =>
                    dest.FromUserId, opt => opt.MapFrom(src =>
                        src.UserId));
            CreateMap<BankTopUpRequest, BankTopUpResponseDTO>();
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

            CreateMap<Voucher, VoucherTopUpResponseDTO>();
            CreateMap<Voucher, TopUpHistory>()
                .ForMember(dest =>
                    dest.CreatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest =>
                    dest.Method, opt => opt.Equals(TopUpHistory.TopUpMethod.Voucher))
                .ForMember(dest =>
                    dest.VoucherId,
                    opt => opt.MapFrom(src => src.Id));


            CreateMap<TopUpHistory, TopUpHistoryResponseDTO>()
                .ForMember(dest =>
                    dest.Method, opt => opt.MapFrom(src =>
                        src.Method.ToString()))
                .ForMember(dest =>
                    dest.Status, opt => opt.MapFrom(src =>
                        src.BankRequest!.Status));

            CreateMap<TransactionHistory, TransactionHistoryResponseDTO>()
                .ForMember(dest =>
                    dest.Status, opt => opt.MapFrom(src =>
                        src.Status.ToString()));

            CreateMap<User, GetTransactionHistoryResponseDTO.UserDTO>();
            CreateMap<User, GetTopUpHistoryResponseDTO.UserDTO>();
            CreateMap<Voucher, GetTopUpHistoryResponseDTO.VoucherDTO>();
            CreateMap<Bank, GetTopUpHistoryResponseDTO.BankDTO>();
            CreateMap<AddBankRequestDTO, Bank>();
            CreateMap<Bank, GetAllBankResponseDTO.BankDTO>();
        }
    }
}
