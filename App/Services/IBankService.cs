using System;
using App.DTOs.Requests;
using App.DTOs.Responses;

namespace App.Services
{
	public interface IBankService
	{
		Task AddNewBank(AddBankRequestDTO dto);
		Task<GetAllBankResponseDTO> GetAllBank();
	}
}

