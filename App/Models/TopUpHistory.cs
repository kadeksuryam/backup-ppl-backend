using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class TopUpHistory : IEntity
    {
        public enum TopUpMethod
        {
            Voucher,
            Bank
        }

        // General
        public uint Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User? From { get; set; }
        public int Amount { get; set; }
        public TopUpMethod? Method { get; set; }

        // Only for bank
        [ForeignKey("BankRequestId")]
        public BankTopUpRequest? BankRequest { get; set; }

        // Only for voucher
        [ForeignKey("VoucherId")]
        public Voucher? Voucher { get; set; }

        // For foreign key
        public uint FromUserId { get; set; }
        public uint? BankRequestId { get; set; }
        public uint? VoucherId { get; set; }
    }
}
