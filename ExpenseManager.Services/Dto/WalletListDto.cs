using ExpenseManager.Storage;

namespace ExpenseManager.Services.Dto
{
    public class WalletListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
