using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Transactions
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        public Transaction Transaction { get; set; }
        private readonly ITransactionService _transactionService;
        public CreateModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost() {
            var check = Transaction;
            _transactionService.Create(Transaction);
            return RedirectToPage("/Transactions/Index");
        }
    }
}
