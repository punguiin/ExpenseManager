using ExpenseManager.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManager.Models
{
    public class WalletModel
    {
        public int Id { get; }
        public string Name { get; set; }
        public Currency Currency { get; set; }
        public List<TransactionModel> Transactions { get; set; }
        public decimal TotalBalance => Transactions?.Sum(t => t.Amount) ?? 0;

        public WalletModel(int id, string name, Currency currency)
        {
            Id = id;
            Name = name;
            Currency = currency;
            Transactions = new List<TransactionModel>();
        }

    }
}
