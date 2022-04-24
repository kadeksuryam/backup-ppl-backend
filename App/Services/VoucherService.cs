using App.Models;
using App.Repositories;
using App.DTOs.Responses;
using AutoMapper;
using App.Helpers;
using System.Net;
using App.DTOs.Requests;

namespace App.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public VoucherService(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        public async Task<GetVoucherResponseDTO> GetByCode(string code)
        {
            Voucher? voucher = await _voucherRepository.GetByCode(code);

            if (voucher == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Voucher not found");
            }

            var response = _mapper.Map<GetVoucherResponseDTO>(voucher);

            return response;
        }

        public async Task<Voucher> UseVoucher(string code)
        {
            Voucher? voucher = await _voucherRepository.GetByCode(code);

            if (voucher == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Voucher not found");
            }

            if (voucher.IsUsed)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "This voucher has been used before");
            }

            voucher.IsUsed = true;
            voucher.UpdatedAt = DateTime.Now;
            Voucher updatedVoucher = await _voucherRepository.Update(voucher);
            return updatedVoucher;
        }

        public async Task AddVoucher(AddVoucherRequestDTO dto)
        {
            Voucher? voucher = await _voucherRepository.GetByCode(dto.Code);

            if (voucher != null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.UnprocessableEntity, "Voucher code must be unique");
            }

            Voucher voucherSave = _mapper.Map<Voucher>(dto);

            await _voucherRepository.Add(voucherSave);
        }
    }
}
