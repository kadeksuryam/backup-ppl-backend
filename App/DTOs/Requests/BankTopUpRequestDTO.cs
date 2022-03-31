using App.Helpers.FormatValidator;
using System.ComponentModel.DataAnnotations;

namespace App.DTOs.Requests
{
    public class BankTopUpRequestDTO
    {
        [Required]
        [Amount]
        public int Amount { get; set; }
        [Required]
        public uint BankId { get; set; }
    }
}