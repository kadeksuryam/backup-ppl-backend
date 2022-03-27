namespace App.Models
{
    public class Voucher : IEntity
    {
        public uint Id { get; set; }
        public string Code { get; set; }
        public uint Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
