using ExpenseManager.Services.Dto;
using ExpenseManager.Storage;

namespace ExpenseManager.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public List<WalletListDto> GetAllWallets()
        {
            var wallets = _repository.GetAllWallets();

            return wallets.Select(w =>
            {
                var transactions = _repository.GetTransactionsByWalletId(w.Id);
                return new WalletListDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Currency = w.Currency,
                    TransactionCount = transactions.Count,
                    TotalBalance = transactions.Sum(t => t.Amount)
                };
            }).ToList();
        }

        public WalletDetailDto? GetWalletById(int walletId)
        {
            var wallet = _repository.GetWalletById(walletId);
            if (wallet == null)
                return null;

            var transactions = _repository.GetTransactionsByWalletId(walletId);

            return new WalletDetailDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Currency = wallet.Currency,
                TotalBalance = transactions.Sum(t => t.Amount),
                Transactions = transactions.Select(t => new TransactionListDto
                {
                    Id = t.Id,
                    Category = t.Category,
                    Description = t.Description,
                    Date = t.Date,
                    Amount = t.Amount
                }).ToList()
            };
        }

        public TransactionDetailDto? GetTransactionById(int transactionId)
        {
            var transaction = _repository.GetTransactionById(transactionId);
            if (transaction == null)
                return null;

            var wallet = _repository.GetWalletById(transaction.WalletId);

            return new TransactionDetailDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Category = transaction.Category,
                Description = transaction.Description,
                Date = transaction.Date,
                IsExpense = transaction.Amount < 0,
                Currency = wallet?.Currency ?? Currency.UAH
            };
        }
    }
}
