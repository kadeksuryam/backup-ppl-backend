using System.ComponentModel.DataAnnotations;

namespace App.DTOs.Requests
{
    public class CreateTransactionRequestDTO
    {
        [Required]
        public uint FromUserId { get; set; }
        [Required]
        public uint ToUserId { get; set; }
        [Required]
        public uint Amount { get; set; }
    }
}
