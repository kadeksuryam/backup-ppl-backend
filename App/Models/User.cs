using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class User : IEntity
    {
        public enum LoginType {
            Standard,
            Google
        }

        public enum UserRole
        {
            Customer,
            Admin
        }

        public uint Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string EncryptedPassword { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public uint Balance { get; set; }

        public uint Exp { get; set; }

        public uint LevelId { get; set; }

        [ForeignKey("LevelId")]
        public Level? Level { get; set; }

        public LoginType Type { get; set; }

        public UserRole Role { get; set; }  
        public List<TopUpHistory>? TopUpHistories { get; set; }
        public List<BankTopUpRequest>? BankTopUpRequests { get; set; }
    } 
}
