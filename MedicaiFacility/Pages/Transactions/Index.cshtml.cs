using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedicaiFacility.RazorPage.Pages.Transactions
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public List<TransactionViewModel> transactionViewModels { get; set; } = new List<TransactionViewModel>();

        public Pager Pager { get; set; }
        public decimal TotalBalance { get; set; }
        private readonly ITransactionService _transactionService;

        private readonly ISystemBalanceService _systemBalanceService;
        public IndexModel(ITransactionService transactionService, ISystemBalanceService systemBalanceService)
        {
            _transactionService = transactionService;
            _systemBalanceService = systemBalanceService;

        }
        public IActionResult OnGet(int pg = 1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            TotalBalance = _systemBalanceService.GetBalance(1).TotalBalance;

            int pageSize = 5;
            var (listTransactionPagination, totalItems) = _transactionService.GetListByPagination(pg, pageSize);
            Pager = new Pager(totalItems, pg, pageSize);
            foreach (var transaction in listTransactionPagination)
            {
                transactionViewModels.Add(new TransactionViewModel
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
                });
            }
            return Page();
        }

        public IActionResult OnPostDeleteById(int id)
        {
            var transaction = _transactionService.GetById(id);
            if (transaction != null)
            {
                transaction.TransactionStatus = "InActive";
                _transactionService.Update(transaction);
            }
            return RedirectToPage("/Transactions/Index");
        }

    }
}
