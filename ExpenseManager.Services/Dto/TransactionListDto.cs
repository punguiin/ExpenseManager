using ExpenseManager.Storage;

namespace ExpenseManager.Services.Dto
{
    public class TransactionListDto
    {
        public int Id { get; set; }
        public TransactionCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
