namespace MedicaiFacility.RazorPage.ViewModel
{
	public class HealthRecordViewModel
	{
		public int RecordId { get; set; }

		public string? PatientName { get; set; }

		public string? UploadedName { get; set; }

		public string FileName { get; set; }

		public string FilePath { get; set; }

		public string TestResult { get; set; }

		public string Diagnosis { get; set; }

		public string Prescription { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public string SharedLink { get; set; }

		public bool? IsActive { get; set; }
	}
}
