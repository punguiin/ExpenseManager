using Microsoft.Extensions.Logging;
using ExpenseManager.Maui.Services;
using ExpenseManager.Maui.ViewModels;
using ExpenseManager.Services;
using ExpenseManager.Storage;

namespace ExpenseManager.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        // Repository
        builder.Services.AddSingleton<IExpenseRepository, ExpenseRepository>();

        // Service
        builder.Services.AddSingleton<IExpenseService, ExpenseService>();

        // Navigation
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // ViewModels
        builder.Services.AddTransient<WalletsViewModel>();
        builder.Services.AddTransient<WalletDetailsViewModel>();
        builder.Services.AddTransient<TransactionDetailsViewModel>();

        // Pages
        builder.Services.AddTransient<WalletsPage>();
        builder.Services.AddTransient<WalletDetailsPage>();
        builder.Services.AddTransient<TransactionDetailsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}
