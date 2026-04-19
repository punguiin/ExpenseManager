using ExpenseManager.Storage;

namespace ExpenseManager.Services.Dto
{
    public class TransactionDetailDto
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsExpense { get; set; }
        public Currency Currency { get; set; }
    }
}
