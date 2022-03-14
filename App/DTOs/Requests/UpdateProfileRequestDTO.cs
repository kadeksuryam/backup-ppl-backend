using System.ComponentModel.DataAnnotations;

namespace App.Services
{
    public class UpdateProfileRequestDTO
    {
        [Required]
        public uint UserId { get; set; }
        [Required]
        public string UserName { get; set; }
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