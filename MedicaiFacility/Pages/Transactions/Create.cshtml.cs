using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicaiFacility.RazorPage.Pages.Transactions
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        public Transaction Transaction { get; set; }
        private readonly ITransactionService _transactionService;
        private readonly ISystemBalanceService _systemBalanceService;
        public decimal TotalBalance { get; set; }
        public List<SelectListItem> UsersOption { get; set; } = new();
        private readonly IUserService _userService;
        public CreateModel(ITransactionService transactionService, IUserService userService,ISystemBalanceService systemBalanceService)
        {
            _transactionService = transactionService;
            _userService = userService;
            _systemBalanceService = systemBalanceService;
        }

        public void OnGet()
        {
            TotalBalance = _systemBalanceService.GetBalance(1).TotalBalance;
            var User = _userService.GetAllUsers();
            UsersOption = User.Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(), // Ép kiểu an toàn
                Text = (x.FullName + " - " + x.Email + " - " + x.PhoneNumber).ToString()
            }).ToList();
        }
        public IActionResult OnPost() {
            var check = Transaction;
            Transaction.TransactionStatus = "Success";
            if (Transaction.Amount> TotalBalance) {
                TempData["ErrorMessage"] = "Overpriced !!!";
                return RedirectToPage("/Transactions/Index");
            }
            Transaction.BalanceId = 1;
            _systemBalanceService.update(1,-Transaction.Amount);
            Transaction.CreatedAt = DateTime.Now;
            Transaction.UpdateAt = DateTime.Now;
            _transactionService.Create(Transaction);
            return RedirectToPage("/Transactions/Index");
        }
    }
}
