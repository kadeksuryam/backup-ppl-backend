using System;
using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
	public class PagingDTO
	{
		[JsonPropertyName("page")]
		public int page { get; set; }
		[JsonPropertyName("size")]
		public int size { get; set; }
		[JsonPropertyName("count")]
		public int count { get; set; }
	}
}

