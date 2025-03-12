using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.Transactions
{
	[BindProperties]
    public class MyTransactionsModel : PageModel
	{
		public List<TransactionViewModel> transactionViewModels { get; set; } = new List<TransactionViewModel>();
		public Pager Pager { get; set; }
		private readonly ITransactionService _transactionService;
		public MyTransactionsModel(ITransactionService transactionService)
		{
			_transactionService = transactionService;
		}
		public void OnGet(int pg = 1)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			int pageSize = 5;
			var (listTransactionPagination, totalItems) = _transactionService.GetListByPaginationWithPatientId(pg, pageSize, userId);
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
		}
	}
}

