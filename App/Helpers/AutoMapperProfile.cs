﻿using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Models;
using AutoMapper;

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

            CreateMap<BankTopUpRequestDTO, BankTopUpRequest>();
            CreateMap<BankTopUpRequest, BankTopUpResponseDTO>()
                .ForMember(dest =>
                    dest.ExpiredDate, opt => opt.MapFrom(src =>
                        src.ExpiredDate.ToString("dd/MM/yyyy, HH:mm")));

            CreateMap<User, GetBankTopUpRequestResponseDTO.UserDTO>();
            CreateMap<Bank, GetBankTopUpRequestResponseDTO.BankDTO>();

            CreateMap<Voucher, GetVoucherResponseDTO>();

            CreateMap<Voucher, VoucherTopUpResponseDTO>();
            CreateMap<Voucher, TopUpHistory>()
                .ForMember(dest =>
                    dest.CreatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest =>
                    dest.Method, opt => opt.Equals(TopUpHistory.TopUpMethod.Voucher))
                .ForMember(dest =>
                    dest.VoucherId,
                    opt => opt.MapFrom(src => src.Id));

            /*
            TopUpHistory history = new()
            {
                Amount = (int)voucher.Amount,
                CreatedAt = now,
                UpdatedAt = now,
                FromUserId = userId,
                Method = TopUpHistory.TopUpMethod.Voucher,
                VoucherId = voucher.Id,
            };
            */
        }
    }
}
