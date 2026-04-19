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

        var storagePath = Path.Combine(FileSystem.AppDataDirectory, "expenses.json");
        builder.Services.AddSingleton<IExpenseRepository>(_ => new ExpenseRepository(storagePath));

        builder.Services.AddSingleton<IExpenseService, ExpenseService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddTransient<WalletsViewModel>();
        builder.Services.AddTransient<WalletDetailsViewModel>();
        builder.Services.AddTransient<WalletEditViewModel>();
        builder.Services.AddTransient<TransactionDetailsViewModel>();
        builder.Services.AddTransient<TransactionEditViewModel>();

        builder.Services.AddTransient<WalletsPage>();
        builder.Services.AddTransient<WalletDetailsPage>();
        builder.Services.AddTransient<WalletEditPage>();
        builder.Services.AddTransient<TransactionDetailsPage>();
        builder.Services.AddTransient<TransactionEditPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
