using ExpenseManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManager.Services
{
    public interface IExpenseService
    {
        List<WalletModel> GetAllWallets();
        WalletModel? GetWalletById(int walletId);
        List<TransactionModel> GetTransactionsByWalletId(int walletId);
    }
}
