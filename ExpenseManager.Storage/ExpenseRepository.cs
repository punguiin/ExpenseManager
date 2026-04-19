using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExpenseManager.Storage
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _gate = new(1, 1);
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private ExpenseStorageState? _state;

        public ExpenseRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<WalletEntity>> GetAllWalletsAsync()
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                return state.Wallets.Select(CloneWallet).ToList();
            }
            finally { _gate.Release(); }
        }

        public async Task<WalletEntity?> GetWalletByIdAsync(int walletId)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                var wallet = state.Wallets.FirstOrDefault(w => w.Id == walletId);
                return wallet == null ? null : CloneWallet(wallet);
            }
            finally { _gate.Release(); }
        }

        public async Task<List<TransactionEntity>> GetTransactionsByWalletIdAsync(int walletId)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                return state.Transactions
                    .Where(t => t.WalletId == walletId)
                    .Select(CloneTransaction)
                    .ToList();
            }
            finally { _gate.Release(); }
        }

        public async Task<TransactionEntity?> GetTransactionByIdAsync(int transactionId)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                var transaction = state.Transactions.FirstOrDefault(t => t.Id == transactionId);
                return transaction == null ? null : CloneTransaction(transaction);
            }
            finally { _gate.Release(); }
        }

        public async Task<WalletEntity> AddWalletAsync(string name, Currency currency)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                int nextId = state.Wallets.Count == 0 ? 1 : state.Wallets.Max(w => w.Id) + 1;
                var wallet = new WalletEntity(nextId, name, currency);
                state.Wallets.Add(wallet);
                await SaveAsync();
                return CloneWallet(wallet);
            }
            finally { _gate.Release(); }
        }

        public async Task UpdateWalletAsync(int walletId, string name, Currency currency)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                var wallet = state.Wallets.FirstOrDefault(w => w.Id == walletId);
                if (wallet == null) return;
                wallet.Name = name;
                wallet.Currency = currency;
                await SaveAsync();
            }
            finally { _gate.Release(); }
        }

        public async Task DeleteWalletAsync(int walletId)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                state.Wallets.RemoveAll(w => w.Id == walletId);
                state.Transactions.RemoveAll(t => t.WalletId == walletId);
                await SaveAsync();
            }
            finally { _gate.Release(); }
        }

        public async Task<TransactionEntity> AddTransactionAsync(int walletId, decimal amount, TransactionCategory category, string description, DateTime date)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                if (state.Wallets.All(w => w.Id != walletId))
                    throw new InvalidOperationException($"Wallet {walletId} not found");

                int nextId = state.Transactions.Count == 0 ? 1 : state.Transactions.Max(t => t.Id) + 1;
                var transaction = new TransactionEntity(nextId, walletId, amount, category, description, date);
                state.Transactions.Add(transaction);
                await SaveAsync();
                return CloneTransaction(transaction);
            }
            finally { _gate.Release(); }
        }

        public async Task UpdateTransactionAsync(int transactionId, decimal amount, TransactionCategory category, string description, DateTime date)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                var transaction = state.Transactions.FirstOrDefault(t => t.Id == transactionId);
                if (transaction == null) return;
                transaction.Amount = amount;
                transaction.Category = category;
                transaction.Description = description;
                transaction.Date = date;
                await SaveAsync();
            }
            finally { _gate.Release(); }
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            await _gate.WaitAsync();
            try
            {
                var state = await EnsureLoadedAsync();
                state.Transactions.RemoveAll(t => t.Id == transactionId);
                await SaveAsync();
            }
            finally { _gate.Release(); }
        }

        private async Task<ExpenseStorageState> EnsureLoadedAsync()
        {
            if (_state != null) return _state;

            if (File.Exists(_filePath))
            {
                await using var stream = File.OpenRead(_filePath);
                _state = await JsonSerializer.DeserializeAsync<ExpenseStorageState>(stream, _jsonOptions)
                         ?? new ExpenseStorageState();
            }
            else
            {
                _state = ExampleDataStorage.CreateSeed();
                await SaveAsync();
            }

            return _state;
        }

        private async Task SaveAsync()
        {
            if (_state == null) return;

            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            await using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, _state, _jsonOptions);
        }

        private static WalletEntity CloneWallet(WalletEntity w) =>
            new(w.Id, w.Name, w.Currency);

        private static TransactionEntity CloneTransaction(TransactionEntity t) =>
            new(t.Id, t.WalletId, t.Amount, t.Category, t.Description, t.Date);
    }
}
