using ExpenseManager.Models;
using ExpenseManager.Storage;

namespace ExpenseManager.Maui
{
    public partial class TransactionDetailsPage : ContentPage
    {
        public TransactionDetailsPage(TransactionModel transaction, Currency currency)
        {
            InitializeComponent();

            string sign = transaction.IsExpense ? "-" : "+";
            decimal absAmount = Math.Abs(transaction.Amount);

            AmountLabel.Text = $"{sign}{absAmount:N2} {currency}";
            CategoryLabel.Text = transaction.Category.ToString();
            DescriptionLabel.Text = transaction.Description;
            DateLabel.Text = transaction.Date.ToString("dd MMMM yyyy");
            TypeLabel.Text = transaction.IsExpense ? "Витрата" : "Дохід";

            if (transaction.IsExpense)
            {
                HeaderFrame.BackgroundColor = Color.FromArgb("#E53935");
                TypeLabel.TextColor = Color.FromArgb("#FFCDD2");
            }
            else
            {
                HeaderFrame.BackgroundColor = Color.FromArgb("#43A047");
                TypeLabel.TextColor = Color.FromArgb("#C8E6C9");
            }
        }
    }
}
