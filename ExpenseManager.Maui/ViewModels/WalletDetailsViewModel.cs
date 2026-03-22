using System.Collections.ObjectModel;
using ExpenseManager.Maui.Services;
using ExpenseManager.Services;
using ExpenseManager.Services.Dto;

namespace ExpenseManager.Maui.ViewModels
{
    public class WalletDetailsViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private readonly INavigationService _navigationService;

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

        private ObservableCollection<TransactionListDto> _transactions = new();
        public ObservableCollection<TransactionListDto> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        private TransactionListDto? _selectedTransaction;
        public TransactionListDto? SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (SetProperty(ref _selectedTransaction, value) && value != null)
                {
                    _navigationService.NavigateToTransactionDetailsAsync(value.Id);
                    SelectedTransaction = null;
                }
            }
        }

        public WalletDetailsViewModel(IExpenseService expenseService, INavigationService navigationService)
        {
            _expenseService = expenseService;
            _navigationService = navigationService;
        }

        public void Load(int walletId)
        {
            var wallet = _expenseService.GetWalletById(walletId);
            if (wallet == null) return;

            Name = wallet.Name;
            CurrencyText = $"Валюта: {wallet.Currency}";
            BalanceText = $"{wallet.TotalBalance:N2} {wallet.Currency}";
            Transactions = new ObservableCollection<TransactionListDto>(wallet.Transactions);
        }
    }
}
