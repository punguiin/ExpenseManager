using System.Windows.Input;
using ExpenseManager.Maui.Commands;
using ExpenseManager.Maui.Services;
using ExpenseManager.Services;
using ExpenseManager.Storage;

namespace ExpenseManager.Maui.ViewModels
{
    public class WalletEditViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private readonly INavigationService _navigationService;

        private int? _walletId;

        private string _title = "Новий гаманець";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public List<Currency> Currencies { get; } = Enum.GetValues<Currency>().ToList();

        private Currency _selectedCurrency = Currency.UAH;
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set => SetProperty(ref _selectedCurrency, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public WalletEditViewModel(IExpenseService expenseService, INavigationService navigationService)
        {
            _expenseService = expenseService;
            _navigationService = navigationService;

            SaveCommand = new AsyncRelayCommand(OnSaveAsync);
            CancelCommand = new AsyncRelayCommand(() => _navigationService.GoBackAsync());
        }

        public async Task InitializeAsync(int? walletId)
        {
            _walletId = walletId;
            Title = walletId.HasValue ? "Редагування гаманця" : "Новий гаманець";

            if (!walletId.HasValue) return;

            var wallet = await _expenseService.GetWalletByIdAsync(walletId.Value);
            if (wallet == null) return;

            Name = wallet.Name;
            SelectedCurrency = wallet.Currency;
        }

        private async Task OnSaveAsync()
        {
            var page = Application.Current?.Windows[0].Page;

            if (string.IsNullOrWhiteSpace(Name))
            {
                if (page != null)
                    await page.DisplayAlertAsync("Помилка", "Введіть назву гаманця", "OK");
                return;
            }

            await RunBusyAsync(async () =>
            {
                if (_walletId.HasValue)
                    await _expenseService.UpdateWalletAsync(_walletId.Value, Name.Trim(), SelectedCurrency);
                else
                    await _expenseService.CreateWalletAsync(Name.Trim(), SelectedCurrency);

                await _navigationService.GoBackAsync();
            });
        }
    }
}
