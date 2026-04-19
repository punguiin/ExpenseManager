namespace ExpenseManager.Storage
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public TransactionEntity() { }

        public TransactionEntity(int id, int walletId, decimal amount, TransactionCategory category, string description, DateTime date)
        {
            Id = id;
            WalletId = walletId;
            Amount = amount;
            Category = category;
            Description = description;
            Date = date;
        }
    }
}
