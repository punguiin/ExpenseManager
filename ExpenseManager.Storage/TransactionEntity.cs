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


    }
}
