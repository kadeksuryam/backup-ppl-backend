using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace if3250_2022_35_cakrawala_backend.Models
{
    public class User
    {
        public uint Id { get; set; }

        public string UserName { get; set; }

        public string EncryptedPassword { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public uint Balance { get; set; }

        public uint Exp { get; set; }

    } 
}
