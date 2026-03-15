using System;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.Services;
using ExpenseManager.Models;

namespace ExpenseManager.Maui
{
    public partial class WalletsPage: ContentPage
    {
        private readonly IExpenseService _expenseService;

        public WalletsPage(IExpenseService expenseService)
        {
            InitializeComponent();
            _expenseService = expenseService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            WalletsListView.ItemsSource = _expenseService.GetAllWallets();
        }

        private async void OnWalletSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var wallet = (WalletModel)e.SelectedItem;
            await Navigation.PushAsync(new WalletDetailsPage(_expenseService, wallet.Id));

            WalletsListView.SelectedItem = null ;
        }
    }
}
