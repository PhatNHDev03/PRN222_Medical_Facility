using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Transactions
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public decimal TotalBalance { get; set; }
        public Transaction Transaction { get; set; }
        private readonly ITransactionService _transactionService;
        private readonly ISystemBalanceService _systemBalanceService;
        public EditModel(ITransactionService transactionService, ISystemBalanceService systemBalanceService)
        {
            _transactionService = transactionService;
            _systemBalanceService = systemBalanceService;
        }
        public void OnGet(int? id)
        {
            TotalBalance = _systemBalanceService.GetBalance(1).TotalBalance;
            Transaction = _transactionService.GetById((int)id);
        }
        public IActionResult OnPost() {
            var totalBalance = _systemBalanceService.GetBalance(1).TotalBalance;
            var exist = _transactionService.GetById(Transaction.TransactionId);
        
            if (Transaction.Amount > totalBalance)
            {
                TempData["ErrorMessage"] = "Overpriced !!!";
                return RedirectToPage("/Transactions/Index");
            }
            _systemBalanceService.update(1, -Transaction.Amount+ exist.Amount);
            Transaction.UpdateAt = DateTime.Now;
            exist.PaymentMethod = Transaction.PaymentMethod;
            exist.Amount = Transaction.Amount;
            exist.TransactionStatus = Transaction.TransactionStatus;
            _transactionService.Update(exist);
            return RedirectToPage("/Transactions/Index");
        }
    }
}
