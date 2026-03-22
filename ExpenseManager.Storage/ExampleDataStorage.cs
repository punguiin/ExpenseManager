using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManager.Storage
{
    internal static class ExampleDataStorage
    {
        public static List<WalletEntity> Wallets { get; } = new List<WalletEntity> {
            new WalletEntity(1, "Готівка", Currency.UAH),
            new WalletEntity(2, "Картка", Currency.EUR),
            new WalletEntity(3, "USD рахунок", Currency.USD)
        };

        public static List<TransactionEntity> Transactions { get; } = new List<TransactionEntity>
        {
            // First wallet transactions
            new TransactionEntity(1, 1, 5000.00m, TransactionCategory.Salary, "Зарплата за січень", new DateTime(2026, 1, 15)),
            new TransactionEntity(2, 1, -250.50m, TransactionCategory.Groceries, "Продукти в АТБ", new DateTime(2026, 1, 16)),
            new TransactionEntity(3, 1, -120.00m, TransactionCategory.Cafe, "Обід в ресторані", new DateTime(2026, 1, 17)),
            new TransactionEntity(4, 1, -85.00m, TransactionCategory.Transport, "Таксі додому", new DateTime(2026, 1, 18)),
            new TransactionEntity(5, 1, -1500.00m,TransactionCategory.Utilities, "Комунальні послуги", new DateTime(2026, 1, 20)),
            new TransactionEntity(6, 1, -350.00m, TransactionCategory.Entertainment, "Квитки в кіно", new DateTime(2026, 1, 21)),
            new TransactionEntity(7, 1, -200.00m, TransactionCategory.Health, "Аптека", new DateTime(2026, 1, 22)),
            new TransactionEntity(8, 1, -750.00m, TransactionCategory.Clothing, "Нова куртка", new DateTime(2026, 1, 23)),
            new TransactionEntity(9, 1, -180.00m, TransactionCategory.Groceries, "Продукти в Сільпо", new DateTime(2026, 1, 25)),
            new TransactionEntity(10, 1, 5000.00m, TransactionCategory.Salary, "Зарплата за лютий", new DateTime(2026, 2, 15)),

            // Second wallet transactions
            new TransactionEntity(11, 2, 12000.00m,TransactionCategory.Salary, "Зарплата на картку", new DateTime(2026, 1, 15)),
            new TransactionEntity(12, 2, -3200.00m,TransactionCategory.Auto, "Заправка авто", new DateTime(2026, 1, 19))
        };
    }
}
