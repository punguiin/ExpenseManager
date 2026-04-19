using System.Collections.ObjectModel;
using System.Windows.Input;
using ExpenseManager.Maui.Commands;
using ExpenseManager.Maui.Services;
using ExpenseManager.Services;
using ExpenseManager.Services.Dto;

namespace ExpenseManager.Maui.ViewModels
{
    public class WalletsViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private readonly INavigationService _navigationService;
        private readonly List<WalletListDto> _allWallets = new();

        public ObservableCollection<WalletListDto> Wallets { get; } = new();

        public List<string> SortOptions { get; } = new()
        {
            "Назва (А→Я)",
            "Назва (Я→А)",
            "Баланс ↑",
            "Баланс ↓",
            "Транзакції ↑",
            "Транзакції ↓"
        };

        private string _selectedSort = "Назва (А→Я)";
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

        private string _walletCountText = string.Empty;
        public string WalletCountText
        {
            get => _walletCountText;
            set => SetProperty(ref _walletCountText, value);
        }

        private WalletListDto? _selectedWallet;
        public WalletListDto? SelectedWallet
        {
            get => _selectedWallet;
            set
            {
                if (SetProperty(ref _selectedWallet, value) && value != null)
                {
                    var target = value;
                    SelectedWallet = null;
                    _ = _navigationService.NavigateToWalletDetailsAsync(target.Id);
                }
            }
        }

        public ICommand AddWalletCommand { get; }
        public ICommand DeleteWalletCommand { get; }
        public ICommand RefreshCommand { get; }

        public WalletsViewModel(IExpenseService expenseService, INavigationService navigationService)
        {
            _expenseService = expenseService;
            _navigationService = navigationService;

            AddWalletCommand = new AsyncRelayCommand(OnAddWalletAsync);
            DeleteWalletCommand = new AsyncRelayCommand<WalletListDto>(OnDeleteWalletAsync);
            RefreshCommand = new AsyncRelayCommand(LoadAsync);
        }

        public Task LoadAsync() => RunBusyAsync(async () =>
        {
            var wallets = await _expenseService.GetAllWalletsAsync();
            _allWallets.Clear();
            _allWallets.AddRange(wallets);
            ApplyFilterAndSort();
            WalletCountText = $"{_allWallets.Count} гаманців";
        });

        private Task OnAddWalletAsync() =>
            _navigationService.NavigateToWalletEditAsync(null);

        private async Task OnDeleteWalletAsync(WalletListDto? wallet)
        {
            if (wallet == null || IsBusy) return;

            var page = Application.Current?.Windows[0].Page;
            if (page == null) return;

            bool confirm = await page.DisplayAlertAsync(
                "Видалити гаманець?",
                $"Гаманець \"{wallet.Name}\" та всі його транзакції будуть видалені.",
                "Видалити", "Скасувати");
            if (!confirm) return;

            await RunBusyAsync(async () =>
            {
                await _expenseService.DeleteWalletAsync(wallet.Id);
                _allWallets.RemoveAll(w => w.Id == wallet.Id);
                ApplyFilterAndSort();
                WalletCountText = $"{_allWallets.Count} гаманців";
            });
        }

        private void ApplyFilterAndSort()
        {
            IEnumerable<WalletListDto> query = _allWallets;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var term = SearchText.Trim();
                query = query.Where(w =>
                    w.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    w.Currency.ToString().Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            query = SelectedSort switch
            {
                "Назва (А→Я)" => query.OrderBy(w => w.Name, StringComparer.CurrentCultureIgnoreCase),
                "Назва (Я→А)" => query.OrderByDescending(w => w.Name, StringComparer.CurrentCultureIgnoreCase),
                "Баланс ↑" => query.OrderBy(w => w.TotalBalance),
                "Баланс ↓" => query.OrderByDescending(w => w.TotalBalance),
                "Транзакції ↑" => query.OrderBy(w => w.TransactionCount),
                "Транзакції ↓" => query.OrderByDescending(w => w.TransactionCount),
                _ => query
            };

            Wallets.Clear();
            foreach (var wallet in query)
                Wallets.Add(wallet);
        }
    }
}
