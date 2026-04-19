using ExpenseManager.Maui.ViewModels;

namespace ExpenseManager.Maui
{
    public partial class WalletsPage : ContentPage
    {
        private readonly WalletsViewModel _viewModel;

        public WalletsPage(WalletsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
