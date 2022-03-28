using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class TransactionHistory : IEntity
    {
        public enum TransactionStatus
        {
            Success,
            Failed
        }

        public uint Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey("FromUserId")]
        public User? From { get; set; }
        [ForeignKey("ToUserId")]
        public User? To { get; set; }
        public uint Amount { get; set; }
        public TransactionStatus? Status { get; set; }

        public uint FromUserId { get; set; }
        public uint ToUserId { get; set; }
    }
}
