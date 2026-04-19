namespace ExpenseManager.Storage
{
    public class ExpenseStorageState
    {
        public List<WalletEntity> Wallets { get; set; } = new();
        public List<TransactionEntity> Transactions { get; set; } = new();
    }
}
