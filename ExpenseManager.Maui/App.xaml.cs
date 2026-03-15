using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
        var walletsPage = Handler!.MauiContext!.Services.GetRequiredService<WalletsPage>();
        return new Window(new NavigationPage(walletsPage));
    }
}