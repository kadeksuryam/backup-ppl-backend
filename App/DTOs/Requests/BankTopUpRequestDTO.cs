namespace App.DTOs.Requests
{
    public class BankTopUpRequestDTO
    {
        public int Amount { get; set; }
        public uint BankId { get; set; }
    }
}