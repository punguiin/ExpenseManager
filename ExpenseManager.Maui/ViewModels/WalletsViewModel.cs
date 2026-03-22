using System.Collections.ObjectModel;
using System.Windows.Input;
using ExpenseManager.Maui.Services;
using ExpenseManager.Services;
using ExpenseManager.Services.Dto;

namespace ExpenseManager.Maui.ViewModels
{
    public class WalletsViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<WalletListDto> _wallets = new();
        public ObservableCollection<WalletListDto> Wallets
        {
            get => _wallets;
            set => SetProperty(ref _wallets, value);
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
                    _navigationService.NavigateToWalletDetailsAsync(value.Id);
                    SelectedWallet = null;
                }
            }
        }

        public WalletsViewModel(IExpenseService expenseService, INavigationService navigationService)
        {
            _expenseService = expenseService;
            _navigationService = navigationService;
            LoadWallets();
        }

        private void LoadWallets()
        {
            var wallets = _expenseService.GetAllWallets();
            Wallets = new ObservableCollection<WalletListDto>(wallets);
            WalletCountText = $"{wallets.Count} гаманців";
        }
    }
}
