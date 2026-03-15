using ExpenseManager.Services;
using ExpenseManager.Models;
using ExpenseManager.Storage;

namespace ExpenseManager.Maui
{
    public partial class WalletDetailsPage : ContentPage
    {
        private readonly IExpenseService _expenseService;
        private readonly WalletModel _wallet;

        public WalletDetailsPage(IExpenseService expenseService, int walletId)
        {
            InitializeComponent();
            _expenseService = expenseService;
            _wallet = _expenseService.GetWalletById(walletId)!;

            NameLabel.Text = _wallet.Name;
            CurrencyLabel.Text = $"Валюта: {_wallet.Currency}";
            BalanceLabel.Text = $"{_wallet.TotalBalance:N2} {_wallet.Currency}";

            TransactionsCollectionView.ItemsSource = _wallet.Transactions;
        }

        private async void OnTransactionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            var transaction = (TransactionModel)e.CurrentSelection[0];
            await Navigation.PushAsync(new TransactionDetailsPage(transaction, _wallet.Currency));

            TransactionsCollectionView.SelectedItem = null;
        }
    }
}
