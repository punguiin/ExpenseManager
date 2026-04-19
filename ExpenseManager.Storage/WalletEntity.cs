namespace ExpenseManager.Storage
{
    public class WalletEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Currency Currency { get; set; }

        public WalletEntity() { }

        public WalletEntity(int id, string name, Currency currency)
        {
            Id = id;
            Name = name;
            Currency = currency;
        }
    }
}
