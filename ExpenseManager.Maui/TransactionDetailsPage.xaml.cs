using System;
using System.Collections.Generic;
using System.Text;
using ExpenseManager.Models;
using ExpenseManager.Storage;

namespace ExpenseManager.Maui
{
    public partial class TransactionDetailsPage: ContentPage
    {
        public TransactionDetailsPage(TransactionModel transaction, Currency currency)
        {
            InitializeComponent();

            AmountLabel.Text = $"Сума: {transaction.Amount:+#;-#;0} {currency}";
            CategoryLabel.Text = $"Категорія: {transaction.Category}";
            DescriptionLabel.Text = $"Опис: {transaction.Description}";
            DateLabel.Text = $"Дата: {transaction.Date:dd.MM.yyyy}";
            TypeLabel.Text = $"Тип: {(transaction.IsExpense ? "Витрата" : "Дохід")}";
        }
    }
}
