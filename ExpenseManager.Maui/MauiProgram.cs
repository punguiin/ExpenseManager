using Microsoft.Extensions.Logging;
using ExpenseManager.Services;

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

        builder.Services.AddSingleton<IExpenseService, ExpenseService>();

        builder.Services.AddTransient<WalletsPage>();
        builder.Services.AddTransient<WalletDetailsPage>();
        builder.Services.AddTransient<TransactionDetailsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}
