using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Models;
using AutoMapper;

namespace App.Helpers
{
    public class AutoMapperProfile : Profile
    {
        const string DateFormat = "dd/MM/yyyy, HH:mm";

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

            CreateMap<BankTopUpRequestDTO, BankTopUpRequest>();
            CreateMap<BankTopUpRequest, BankTopUpResponseDTO>()
                .ForMember(dest =>
                    dest.ExpiredDate, opt => opt.MapFrom(src =>
                        src.ExpiredDate.ToString(DateFormat)));

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
                    dest.CreatedAt, opt => opt.MapFrom(src =>
                        src.CreatedAt.ToString(DateFormat)))
                .ForMember(dest =>
                    dest.UpdatedAt, opt => opt.MapFrom(src =>
                        src.UpdatedAt.ToString(DateFormat)))
                .ForMember(dest =>
                    dest.Method, opt => opt.MapFrom(src =>
                        src.Method.ToString()));

            CreateMap<TransactionHistory, TransactionHistoryResponseDTO>()
                .ForMember(dest =>
                    dest.CreatedAt, opt => opt.MapFrom(src =>
                        src.CreatedAt.ToString(DateFormat)))
                .ForMember(dest =>
                    dest.UpdatedAt, opt => opt.MapFrom(src =>
                        src.UpdatedAt.ToString(DateFormat)))
                .ForMember(dest =>
                    dest.Status, opt => opt.MapFrom(src =>
                        src.Status.ToString()));
        }
    }
}
