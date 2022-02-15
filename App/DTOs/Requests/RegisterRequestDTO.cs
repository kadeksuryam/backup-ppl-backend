using System.ComponentModel.DataAnnotations;

namespace App.DTOs.Requests
{
    public class RegisterRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
