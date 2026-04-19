using System.Globalization;
using System.Windows.Input;
using ExpenseManager.Maui.Commands;
using ExpenseManager.Maui.Services;
using ExpenseManager.Services;
using ExpenseManager.Storage;

namespace ExpenseManager.Maui.ViewModels
{
    public class TransactionEditViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private readonly INavigationService _navigationService;

        private int _walletId;
        private int? _transactionId;

        private string _title = "Нова транзакція";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _amountText = "0";
        public string AmountText
        {
            get => _amountText;
            set => SetProperty(ref _amountText, value);
        }

        private bool _isExpense = true;
        public bool IsExpense
        {
            get => _isExpense;
            set => SetProperty(ref _isExpense, value);
        }

        public List<TransactionCategory> Categories { get; } =
            Enum.GetValues<TransactionCategory>().ToList();

        private TransactionCategory _selectedCategory = TransactionCategory.Other;
        public TransactionCategory SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _date = DateTime.Today;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TransactionEditViewModel(IExpenseService expenseService, INavigationService navigationService)
        {
            _expenseService = expenseService;
            _navigationService = navigationService;

            SaveCommand = new AsyncRelayCommand(OnSaveAsync);
            CancelCommand = new AsyncRelayCommand(() => _navigationService.GoBackAsync());
        }

        public async Task InitializeAsync(int walletId, int? transactionId)
        {
            _walletId = walletId;
            _transactionId = transactionId;
            Title = transactionId.HasValue ? "Редагування транзакції" : "Нова транзакція";

            if (!transactionId.HasValue) return;

            var transaction = await _expenseService.GetTransactionByIdAsync(transactionId.Value);
            if (transaction == null) return;

            IsExpense = transaction.IsExpense;
            AmountText = Math.Abs(transaction.Amount).ToString("0.##", CultureInfo.InvariantCulture);
            SelectedCategory = transaction.Category;
            Description = transaction.Description;
            Date = transaction.Date;
        }

        private async Task OnSaveAsync()
        {
            var page = Application.Current?.Windows[0].Page;

            var normalized = (AmountText ?? "0").Replace(',', '.').Trim();
            if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount) || amount < 0)
            {
                if (page != null)
                    await page.DisplayAlertAsync("Помилка", "Введіть коректну невід'ємну суму", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                if (page != null)
                    await page.DisplayAlertAsync("Помилка", "Введіть опис транзакції", "OK");
                return;
            }

            decimal signedAmount = IsExpense ? -Math.Abs(amount) : Math.Abs(amount);

            await RunBusyAsync(async () =>
            {
                if (_transactionId.HasValue)
                    await _expenseService.UpdateTransactionAsync(_transactionId.Value, signedAmount, SelectedCategory, Description.Trim(), Date);
                else
                    await _expenseService.CreateTransactionAsync(_walletId, signedAmount, SelectedCategory, Description.Trim(), Date);

                await _navigationService.GoBackAsync();
            });
        }
    }
}
