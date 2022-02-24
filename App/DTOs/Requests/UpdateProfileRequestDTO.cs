using System.ComponentModel.DataAnnotations;

namespace App.Services
{
    public class UpdateProfileRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string NewDisplayName { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}