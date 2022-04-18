using System;
using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{

	public class GetAllBankResponseDTO
	{

		public class BankDTO
		{
			[JsonPropertyName("id")]
			public uint Id { get; set; }
			[JsonPropertyName("name")]
			public string? Name { get; set; }
			[JsonPropertyName("account_number")]
			public long AccountNumber { get; set; }
		}


		[JsonPropertyName("banks")]
		public List<BankDTO> Banks { get; set; } = new List<BankDTO>();
	}
}

