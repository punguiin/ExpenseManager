using ExpenseManager.Storage;

namespace ExpenseManager.Services.Dto
{
    public class WalletDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public decimal TotalBalance { get; set; }
        public List<TransactionListDto> Transactions { get; set; } = new();
    }
}
