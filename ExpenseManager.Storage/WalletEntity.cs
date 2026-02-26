using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManager.Storage
{
    public class WalletEntity
    {
        public int Id { get; }
        public string Name { get; set; }
        public Currency Currency { get; set; }

        public WalletEntity(int id, string name, Currency currency)
        {
            Id = id;
            Name = name;
            Currency = currency;
        }
    }
}
