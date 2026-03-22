using ExpenseManager.Services.Dto;

namespace ExpenseManager.Services
{
    public interface IExpenseService
    {
        List<WalletListDto> GetAllWallets();
        WalletDetailDto? GetWalletById(int walletId);
        TransactionDetailDto? GetTransactionById(int transactionId);
    }
}
