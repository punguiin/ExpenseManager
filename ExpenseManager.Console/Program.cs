using ExpenseManager.Services;
using ExpenseManager.Models;

namespace ExpenseManager.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            var service = new ExpenseService();

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Менеджер витрат ===\n");

                var wallets = service.GetAllWallets();
                System.Console.WriteLine("Гаманці:");
                for (int i = 0; i < wallets.Count; i++)
                {
                    System.Console.WriteLine($"  {i + 1}. {wallets[i].Name} ({wallets[i].Currency})");
                }

                System.Console.WriteLine($"\n  0. Вийти");
                System.Console.Write("\nВиберіть гаманець: ");
                var input = System.Console.ReadLine();

                if (input == "0") break;

                if (!int.TryParse(input, out int walletIndex) || walletIndex < 1 || walletIndex > wallets.Count)
                {
                    System.Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу...");
                    System.Console.ReadKey();
                    continue;
                }

                var wallet = service.GetWalletById(wallets[walletIndex - 1].Id);
                ShowWalletDetails(wallet!);
            }
        }

        static void ShowWalletDetails(WalletModel wallet)
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine($"--- {wallet.Name} ---");
                System.Console.WriteLine($"Валюта: {wallet.Currency}");
                System.Console.WriteLine($"Баланс: {wallet.TotalBalance:F2} {wallet.Currency}");
                System.Console.WriteLine($"\nТранзакції:");

                if (wallet.Transactions.Count == 0)
                {
                    System.Console.WriteLine("  Немає транзакцій.");
                }
                else
                {
                    for (int i = 0; i < wallet.Transactions.Count; i++)
                    {
                        var t = wallet.Transactions[i];
                        string sign = t.IsExpense ? "" : "+";
                        System.Console.WriteLine($"  {i + 1}. {sign}{t.Amount:F2}  {t.Category,-15} {t.Description}");
                    }
                }

                System.Console.WriteLine($"\n  b - Назад до списку гаманців");
                System.Console.WriteLine($"  0 - Вийти");
                System.Console.Write("\nВиберіть транзакцію для деталей: ");
                var input = System.Console.ReadLine();

                if (input?.ToLower() == "b") break;
                if (input == "0") Environment.Exit(0);

                if (!int.TryParse(input, out int tIndex) || tIndex < 1 || tIndex > wallet.Transactions.Count)
                {
                    System.Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу...");
                    System.Console.ReadKey();
                    continue;
                }

                ShowTransactionDetails(wallet.Transactions[tIndex - 1], wallet.Currency);
            }
        }

        static void ShowTransactionDetails(TransactionModel transaction, Storage.Currency currency)
        {
            System.Console.Clear();
            System.Console.WriteLine("--- Деталі транзакції ---");
            System.Console.WriteLine($"Сума: {(transaction.IsExpense ? "" : "+")}{transaction.Amount:F2} {currency}");
            System.Console.WriteLine($"Категорія: {transaction.Category}");
            System.Console.WriteLine($"Опис: {transaction.Description}");
            System.Console.WriteLine($"Дата: {transaction.Date:dd.MM.yyyy}");
            System.Console.WriteLine($"Тип: {(transaction.IsExpense ? "Витрата" : "Дохід")}");

            System.Console.WriteLine("\nНатисніть будь-яку клавішу для повернення або 0 для виходу...");
            var key = System.Console.ReadKey();
            if (key.KeyChar == '0') Environment.Exit(0);
        }
    }
}