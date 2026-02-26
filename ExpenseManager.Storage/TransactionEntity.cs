using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManager.Storage
{
    public class TransactionEntity
    {
        public int Id { get; }
        public int WalletId { get; }
        public decimal Amount { get; set; }
        public TransactionCategory Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

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
