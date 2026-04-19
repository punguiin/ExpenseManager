using System.Collections.ObjectModel;
using System.Windows.Input;
using ExpenseManager.Maui.Commands;
using ExpenseManager.Maui.Services;
using ExpenseManager.Services;
using ExpenseManager.Services.Dto;
using ExpenseManager.Storage;

namespace ExpenseManager.Maui.ViewModels
{
    public class WalletDetailsViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private readonly INavigationService _navigationService;
        private readonly List<TransactionListDto> _allTransactions = new();

        private int _walletId;
        private Currency _currency;

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _currencyText = string.Empty;
        public string CurrencyText
        {
            get => _currencyText;
            set => SetProperty(ref _currencyText, value);
        }

        private string _balanceText = string.Empty;
        public string BalanceText
        {
            get => _balanceText;
            set => SetProperty(ref _balanceText, value);
        }

        public ObservableCollection<TransactionListDto> Transactions { get; } = new();

        public List<string> SortOptions { get; } = new()
        {
            "Дата ↓",
            "Дата ↑",
            "Сума ↓",
            "Сума ↑",
            "Категорія"
        };

        private string _selectedSort = "Дата ↓";
        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                if (SetProperty(ref _selectedSort, value))
                    ApplyFilterAndSort();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    ApplyFilterAndSort();
            }
        }

        private TransactionListDto? _selectedTransaction;
        public TransactionListDto? SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (SetProperty(ref _selectedTransaction, value) && value != null)
                {
                    var target = value;
                    SelectedTransaction = null;
                    _ = _navigationService.NavigateToTransactionDetailsAsync(target.Id);
                }
            }
        }

        public ICommand BackCommand { get; }
        public ICommand EditWalletCommand { get; }
        public ICommand DeleteWalletCommand { get; }
        public ICommand AddTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }

        public WalletDetailsViewModel(IExpenseService expenseService, INavigationService navigationService)
        {
            _expenseService = expenseService;
            _navigationService = navigationService;

            BackCommand = new AsyncRelayCommand(() => _navigationService.GoBackAsync());
            EditWalletCommand = new AsyncRelayCommand(OnEditWalletAsync);
            DeleteWalletCommand = new AsyncRelayCommand(OnDeleteWalletAsync);
            AddTransactionCommand = new AsyncRelayCommand(OnAddTransactionAsync);
            DeleteTransactionCommand = new AsyncRelayCommand<TransactionListDto>(OnDeleteTransactionAsync);
        }

        public void Initialize(int walletId) => _walletId = walletId;

        public Task LoadAsync() => RunBusyAsync(async () =>
        {
            var wallet = await _expenseService.GetWalletByIdAsync(_walletId);
            if (wallet == null)
            {
                await _navigationService.GoBackAsync();
                return;
            }

            _currency = wallet.Currency;
            Name = wallet.Name;
            CurrencyText = $"Валюта: {wallet.Currency}";
            BalanceText = $"{wallet.TotalBalance:N2} {wallet.Currency}";

            _allTransactions.Clear();
            _allTransactions.AddRange(wallet.Transactions);
            ApplyFilterAndSort();
        });

        private Task OnEditWalletAsync() =>
            _navigationService.NavigateToWalletEditAsync(_walletId);

        private async Task OnDeleteWalletAsync()
        {
            var page = Application.Current?.Windows[0].Page;
            if (page == null) return;

            bool confirm = await page.DisplayAlertAsync(
                "Видалити гаманець?",
                $"Гаманець \"{Name}\" та всі його транзакції будуть видалені.",
                "Видалити", "Скасувати");
            if (!confirm) return;

            await RunBusyAsync(async () =>
            {
                await _expenseService.DeleteWalletAsync(_walletId);
                await _navigationService.GoBackAsync();
            });
        }

        private Task OnAddTransactionAsync() =>
            _navigationService.NavigateToTransactionEditAsync(_walletId, null);

        private async Task OnDeleteTransactionAsync(TransactionListDto? transaction)
        {
            if (transaction == null) return;

            var page = Application.Current?.Windows[0].Page;
            if (page == null) return;

            bool confirm = await page.DisplayAlertAsync(
                "Видалити транзакцію?",
                $"Транзакцію \"{transaction.Description}\" буде видалено.",
                "Видалити", "Скасувати");
            if (!confirm) return;

            await RunBusyAsync(async () =>
            {
                await _expenseService.DeleteTransactionAsync(transaction.Id);
                await LoadAfterMutationAsync();
            });
        }

        private async Task LoadAfterMutationAsync()
        {
            var wallet = await _expenseService.GetWalletByIdAsync(_walletId);
            if (wallet == null) return;

            BalanceText = $"{wallet.TotalBalance:N2} {wallet.Currency}";
            _allTransactions.Clear();
            _allTransactions.AddRange(wallet.Transactions);
            ApplyFilterAndSort();
        }

        private void ApplyFilterAndSort()
        {
            IEnumerable<TransactionListDto> query = _allTransactions;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var term = SearchText.Trim();
                query = query.Where(t =>
                    t.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    t.Category.ToString().Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            query = SelectedSort switch
            {
                "Дата ↓" => query.OrderByDescending(t => t.Date),
                "Дата ↑" => query.OrderBy(t => t.Date),
                "Сума ↓" => query.OrderByDescending(t => t.Amount),
                "Сума ↑" => query.OrderBy(t => t.Amount),
                "Категорія" => query.OrderBy(t => t.Category.ToString()),
                _ => query
            };

            Transactions.Clear();
            foreach (var transaction in query)
                Transactions.Add(transaction);
        }
    }
}
