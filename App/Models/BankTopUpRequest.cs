using App.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class BankTopUpRequest : IEntity
    {
        public uint Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int Amount { get; set; }

        [ForeignKey("ToBankId")]
        public Bank? Bank { get; set; }

        [ForeignKey("FromUserId")]
        public User? From { get; set; }
        public RequestStatus? Status { get; set; }

        // For connection
        public TopUpHistory? History { get; set; }
        public uint FromUserId { get; set; }
        public uint BankId { get; set; }

    }
}