using System.Windows.Input;
using ExpenseManager.Maui.Commands;
using ExpenseManager.Maui.Services;
using ExpenseManager.Services;

namespace ExpenseManager.Maui.ViewModels
{
    public class TransactionDetailsViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private readonly INavigationService _navigationService;

        private int _transactionId;
        private int _walletId;

        private string _amountText = string.Empty;
        public string AmountText
        {
            get => _amountText;
            set => SetProperty(ref _amountText, value);
        }

        private string _typeText = string.Empty;
        public string TypeText
        {
            get => _typeText;
            set => SetProperty(ref _typeText, value);
        }

        private string _categoryText = string.Empty;
        public string CategoryText
        {
            get => _categoryText;
            set => SetProperty(ref _categoryText, value);
        }

        private string _descriptionText = string.Empty;
        public string DescriptionText
        {
            get => _descriptionText;
            set => SetProperty(ref _descriptionText, value);
        }

        private string _dateText = string.Empty;
        public string DateText
        {
            get => _dateText;
            set => SetProperty(ref _dateText, value);
        }

        private Color _headerColor = Color.FromArgb("#43A047");
        public Color HeaderColor
        {
            get => _headerColor;
            set => SetProperty(ref _headerColor, value);
        }

        private Color _typeLabelColor = Color.FromArgb("#C8E6C9");
        public Color TypeLabelColor
        {
            get => _typeLabelColor;
            set => SetProperty(ref _typeLabelColor, value);
        }

        public ICommand BackCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public TransactionDetailsViewModel(IExpenseService expenseService, INavigationService navigationService)
        {
            _expenseService = expenseService;
            _navigationService = navigationService;

            BackCommand = new AsyncRelayCommand(() => _navigationService.GoBackAsync());
            EditCommand = new AsyncRelayCommand(OnEditAsync);
            DeleteCommand = new AsyncRelayCommand(OnDeleteAsync);
        }

        public void Initialize(int transactionId) => _transactionId = transactionId;

        public Task LoadAsync() => RunBusyAsync(async () =>
        {
            var transaction = await _expenseService.GetTransactionByIdAsync(_transactionId);
            if (transaction == null)
            {
                await _navigationService.GoBackAsync();
                return;
            }

            _walletId = transaction.WalletId;

            string sign = transaction.IsExpense ? "-" : "+";
            decimal absAmount = Math.Abs(transaction.Amount);

            AmountText = $"{sign}{absAmount:N2} {transaction.Currency}";
            CategoryText = transaction.Category.ToString();
            DescriptionText = transaction.Description;
            DateText = transaction.Date.ToString("dd MMMM yyyy");
            TypeText = transaction.IsExpense ? "Витрата" : "Дохід";

            if (transaction.IsExpense)
            {
                HeaderColor = Color.FromArgb("#E53935");
                TypeLabelColor = Color.FromArgb("#FFCDD2");
            }
            else
            {
                HeaderColor = Color.FromArgb("#43A047");
                TypeLabelColor = Color.FromArgb("#C8E6C9");
            }
        });

        private Task OnEditAsync() =>
            _navigationService.NavigateToTransactionEditAsync(_walletId, _transactionId);

        private async Task OnDeleteAsync()
        {
            var page = Application.Current?.Windows[0].Page;
            if (page == null) return;

            bool confirm = await page.DisplayAlertAsync(
                "Видалити транзакцію?",
                "Транзакцію буде видалено.",
                "Видалити", "Скасувати");
            if (!confirm) return;

            await RunBusyAsync(async () =>
            {
                await _expenseService.DeleteTransactionAsync(_transactionId);
                await _navigationService.GoBackAsync();
            });
        }
    }
}
