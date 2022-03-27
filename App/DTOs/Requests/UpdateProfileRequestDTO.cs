using System.ComponentModel.DataAnnotations;

namespace App.Services
{
    public class UpdateProfileRequestDTO
    {
        [Required]
        [DisplayName]
        public string NewDisplayName { get; set; }
        [Required]
        [Password]
        public string OldPassword { get; set; }
        [Required]
        [Password]
        public string NewPassword { get; set; }
    }
}