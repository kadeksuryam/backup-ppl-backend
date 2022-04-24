using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.DTOs.Requests
{
	public class AddVoucherRequestDTO
	{
		[JsonPropertyName("code")]
		[Required]
		public string Code { get; set; }
		[JsonPropertyName("amount")]
		[Required]
		public uint Amount { get; set; }
	}
}

