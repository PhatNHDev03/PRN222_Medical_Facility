namespace MedicaiFacility.RazorPage.ViewModel
{
	public class AppoinmentViewModel
	{
		public int AppointmentId { get; set; }
		public int? PatientId { get; set; }
		public string? PatientName { get; set; }
        public string? PatientPhone { get; set; }
        public int? ExpertId { get; set; } 
		public string? ExpertName { get; set; }
        public string? ExpertPhone { get; set; }
        public int? FacilityId { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string Status { get; set; }

		public int? TransactionId { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
		//facility
		public string FacilityName { get; set; }
		public string Address { get; set; }
		//transaction
	
		public string PaymentMethod { get; set; }
		public decimal Amount { get; set; }
		public string TransactionStatus { get; set; }
	
	}
}
