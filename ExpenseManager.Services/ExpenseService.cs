using ExpenseManager.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.Models;
using ExpenseManager.Services;

namespace ExpenseManager.Services
{
    public class ExpenseService
    {
        public List<WalletModel> GetAllWallets()
        {
            return ExampleDataStorage.Wallets
                .Select(w => new WalletModel(w.Id, w.Name, w.Currency))
                .ToList();
        }

        public WalletModel? GetWalletById(int walletId)
        {
            var entity = ExampleDataStorage.Wallets.FirstOrDefault(w => w.Id == walletId);
            if (entity == null) {
                return null;
            }

            var wallet = new WalletModel(entity.Id, entity.Name, entity.Currency);
            wallet.Transactions = GetTransactionsByWalletId(walletId);
            return wallet;
        }

        public List<TransactionModel> GetTransactionsByWalletId(int walletId)
        { 
            return ExampleDataStorage.Transactions
                .Where(t => t.WalletId == walletId)
                .Select(t => new TransactionModel(t.Id, t.WalletId, t.Amount, t.Category, t.Description, t.Date))
                .ToList();
        }
    }
}
