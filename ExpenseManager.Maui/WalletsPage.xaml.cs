using ExpenseManager.Services;
using ExpenseManager.Models;

namespace ExpenseManager.Maui
{
    public partial class WalletsPage : ContentPage
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
            var wallets = _expenseService.GetAllWallets();
            WalletsCollectionView.ItemsSource = wallets;
            WalletCountLabel.Text = $"{wallets.Count} гаманців";
            WalletsCollectionView.SelectedItem = null;
        }

        private async void OnWalletSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            var wallet = (WalletModel)e.CurrentSelection[0];
            await Navigation.PushAsync(new WalletDetailsPage(_expenseService, wallet.Id));

            WalletsCollectionView.SelectedItem = null;
        }
    }
}
