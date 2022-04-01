using App.Helpers.FormatValidator;
﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.Services
{
    public class UpdateProfileRequestDTO
    {
        [JsonPropertyName("new_display_name")]
        public string? NewDisplayName { get; set; }
        [JsonPropertyName("old_password")]
        public string? OldPassword { get; set; }
        [JsonPropertyName("new_password")]
        public string? NewPassword { get; set; }
    }
}