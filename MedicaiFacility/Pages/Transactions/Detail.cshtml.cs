using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Transactions
{
    public class DetailModel : PageModel
    {
        public TransactionViewModel TransactionViewModel { get; set; }
        private readonly ITransactionService _transactionService;
        public DetailModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public void OnGet(int? id)
        {
            var transaction = _transactionService.GetById((int)id);
            TransactionViewModel = new TransactionViewModel
            {
                TransactionId = transaction.TransactionId,
                UserName = transaction.User.FullName,
                UserEmail = transaction.User.Email,
                NumberPhone = transaction.User.PhoneNumber,
                PaymentMethod = transaction.PaymentMethod,
                BankAccount = transaction.User.BankAccount,
                Amount = transaction.Amount,
                TransactionStatus = transaction.TransactionStatus,
                CreatedAt = transaction.CreatedAt,
                UpdateAt = transaction.UpdateAt,
                TransactionType = transaction.TransactionType
            };
        }
    }
}
