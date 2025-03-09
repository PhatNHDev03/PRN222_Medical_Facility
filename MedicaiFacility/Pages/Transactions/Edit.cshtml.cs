using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Transactions
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public Transaction Transaction { get; set; }
        private readonly ITransactionService _transactionService;
        public EditModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public void OnGet(int? id)
        {
            Transaction = _transactionService.GetById((int)id);
        }
        public IActionResult OnPost() {
            var check = Transaction;
            Transaction.UpdateAt = DateTime.Now;
            _transactionService.Update(Transaction);
            return RedirectToPage("/Transactions/Index");
        }
    }
}
