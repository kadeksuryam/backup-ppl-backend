﻿using App.DTOs.Requests;
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

            CreateMap<BankTopUpRequestDTO, BankTopUpRequest>();
            CreateMap<BankTopUpRequest, BankTopUpResponseDTO>()
                .ForMember(dest =>
                    dest.ExpiredDate, opt => opt.MapFrom(src =>
                        src.ExpiredDate.ToString("dd/MM/yyyy, HH:mm")));
        }

    }
}
