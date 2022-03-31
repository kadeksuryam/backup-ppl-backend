using App.Models;
using App.Repositories;
using App.DTOs.Responses;
using AutoMapper;

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

            var response = _mapper.Map<GetVoucherResponseDTO>(voucher);

            return response;
        }
    }
}
