using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.DTOs.Requests
{
	public class AddBankRequestDTO
	{
		[JsonPropertyName("name")]
		[Required]
		public string? Name { get; set; }
		[JsonPropertyName("account_number")]
		[Required]
		public long AccountNumber { get; set; }
	}
}

