using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class User : IEntity
    {
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

    } 
}
