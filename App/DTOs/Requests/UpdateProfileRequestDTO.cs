using System.ComponentModel.DataAnnotations;

namespace App.Services
{
    public class UpdateProfileRequestDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [DisplayName]
        public string NewDisplayName { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [Password]
        public string NewPassword { get; set; }
    }
}