namespace MedicaiFacility.RazorPage.ViewModel
{
	public class TransactionViewModel
	{
		public int TransactionId { get; set; }

		public string? UserName { get; set; }
		public string? UserEmail { get; set; }
		public string? NumberPhone { get; set; }
		public string BankAccount { get; set; }
		public string PaymentMethod { get; set; }

		public decimal Amount { get; set; }

		public string TransactionStatus { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdateAt { get; set; }

		public string TransactionType { get; set; }
	}
}
