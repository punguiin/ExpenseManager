using System;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.Services;
using ExpenseManager.Models;

namespace ExpenseManager.Maui
{
    public partial class WalletDetailsPage: ContentPage
    {
        private readonly IExpenseService _expenseService;
        private WalletModel _wallet;

        public WalletDetailsPage(IExpenseService expenseService, int walletId)
        {
            InitializeComponent();
            _expenseService = expenseService;
            _wallet = _expenseService.GetWalletById(walletId)!;

            NameLabel.Text = _wallet.Name;
            CurrencyLabel.Text = $"Валюта: {_wallet.Currency}";
            BalanceLabel.Text = $"Баланс: {_wallet.TotalBalance} {_wallet.Currency}";

            TransactionsListView.ItemsSource = _wallet.Transactions;
        }

        private async void OnTransactionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var transaction = (TransactionModel)e.SelectedItem;
            await Navigation.PushAsync(new TransactionDetailsPage(transaction, _wallet.Currency));

            TransactionsListView.SelectedItem = null;
        }
    }
}
