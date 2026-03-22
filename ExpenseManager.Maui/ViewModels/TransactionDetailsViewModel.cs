using ExpenseManager.Services;
using ExpenseManager.Services.Dto;

namespace ExpenseManager.Maui.ViewModels
{
    public class TransactionDetailsViewModel : BaseViewModel
    {
        private readonly IExpenseService _expenseService;

        private string _amountText = string.Empty;
        public string AmountText
        {
            get => _amountText;
            set => SetProperty(ref _amountText, value);
        }

        private string _typeText = string.Empty;
        public string TypeText
        {
            get => _typeText;
            set => SetProperty(ref _typeText, value);
        }

        private string _categoryText = string.Empty;
        public string CategoryText
        {
            get => _categoryText;
            set => SetProperty(ref _categoryText, value);
        }

        private string _descriptionText = string.Empty;
        public string DescriptionText
        {
            get => _descriptionText;
            set => SetProperty(ref _descriptionText, value);
        }

        private string _dateText = string.Empty;
        public string DateText
        {
            get => _dateText;
            set => SetProperty(ref _dateText, value);
        }

        private Color _headerColor = Color.FromArgb("#43A047");
        public Color HeaderColor
        {
            get => _headerColor;
            set => SetProperty(ref _headerColor, value);
        }

        private Color _typeLabelColor = Color.FromArgb("#C8E6C9");
        public Color TypeLabelColor
        {
            get => _typeLabelColor;
            set => SetProperty(ref _typeLabelColor, value);
        }

        public TransactionDetailsViewModel(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public void Load(int transactionId)
        {
            var transaction = _expenseService.GetTransactionById(transactionId);
            if (transaction == null) return;

            string sign = transaction.IsExpense ? "-" : "+";
            decimal absAmount = Math.Abs(transaction.Amount);

            AmountText = $"{sign}{absAmount:N2} {transaction.Currency}";
            CategoryText = transaction.Category.ToString();
            DescriptionText = transaction.Description;
            DateText = transaction.Date.ToString("dd MMMM yyyy");
            TypeText = transaction.IsExpense ? "Витрата" : "Дохід";

            if (transaction.IsExpense)
            {
                HeaderColor = Color.FromArgb("#E53935");
                TypeLabelColor = Color.FromArgb("#FFCDD2");
            }
            else
            {
                HeaderColor = Color.FromArgb("#43A047");
                TypeLabelColor = Color.FromArgb("#C8E6C9");
            }
        }
    }
}
