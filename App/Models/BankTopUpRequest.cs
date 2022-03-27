namespace App.Models
{
    public class BankTopUpRequest : IEntity
    {
        public uint Id { get; set; }
        public int VirtualAccountNumber { get; set; }
        public DateTime? DueDate { get; set; }
        public int Value { get; set; }
        public string? BankName { get; set; }
        public User? From { get; set; }

        // For connection
        public TopUpHistory? History { get; set; }
        public uint FromUserId { get; set; }

    }
}