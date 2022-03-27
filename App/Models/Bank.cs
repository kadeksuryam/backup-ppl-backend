namespace App.Models
{
    public class Bank : IEntity
    {
        public uint Id { get; set; }
        public string? Name { get; set; }
        public long AccountNumber { get; set; }
        public List<BankTopUpRequest>? BankTopUpRequests { get; set; }

    }
}