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

        public async Task<List<WalletListDto>> GetAllWalletsAsync()
        {
            var wallets = await _repository.GetAllWalletsAsync();
            var result = new List<WalletListDto>(wallets.Count);

            foreach (var wallet in wallets)
            {
                var transactions = await _repository.GetTransactionsByWalletIdAsync(wallet.Id);
                result.Add(new WalletListDto
                {
                    Id = wallet.Id,
                    Name = wallet.Name,
                    Currency = wallet.Currency,
                    TransactionCount = transactions.Count,
                    TotalBalance = transactions.Sum(t => t.Amount)
                });
            }

            return result;
        }

        public async Task<WalletDetailDto?> GetWalletByIdAsync(int walletId)
        {
            var wallet = await _repository.GetWalletByIdAsync(walletId);
            if (wallet == null)
                return null;

            var transactions = await _repository.GetTransactionsByWalletIdAsync(walletId);

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

        public async Task<TransactionDetailDto?> GetTransactionByIdAsync(int transactionId)
        {
            var transaction = await _repository.GetTransactionByIdAsync(transactionId);
            if (transaction == null)
                return null;

            var wallet = await _repository.GetWalletByIdAsync(transaction.WalletId);

            return new TransactionDetailDto
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Amount = transaction.Amount,
                Category = transaction.Category,
                Description = transaction.Description,
                Date = transaction.Date,
                IsExpense = transaction.Amount < 0,
                Currency = wallet?.Currency ?? Currency.UAH
            };
        }

        public async Task<int> CreateWalletAsync(string name, Currency currency)
        {
            var wallet = await _repository.AddWalletAsync(name, currency);
            return wallet.Id;
        }

        public Task UpdateWalletAsync(int walletId, string name, Currency currency) =>
            _repository.UpdateWalletAsync(walletId, name, currency);

        public Task DeleteWalletAsync(int walletId) =>
            _repository.DeleteWalletAsync(walletId);

        public async Task<int> CreateTransactionAsync(int walletId, decimal amount, TransactionCategory category, string description, DateTime date)
        {
            var transaction = await _repository.AddTransactionAsync(walletId, amount, category, description, date);
            return transaction.Id;
        }

        public Task UpdateTransactionAsync(int transactionId, decimal amount, TransactionCategory category, string description, DateTime date) =>
            _repository.UpdateTransactionAsync(transactionId, amount, category, description, date);

        public Task DeleteTransactionAsync(int transactionId) =>
            _repository.DeleteTransactionAsync(transactionId);
    }
}
