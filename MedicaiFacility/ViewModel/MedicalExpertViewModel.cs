namespace MedicaiFacility.RazorPage.ViewModel
{
    public class MedicalExpertViewModel
    {
        public int? ExpertId { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Specialization { get; set; }
        public int? ExperienceYears { get; set; }
        public string? Department { get; set; }
        public decimal? PriceBooking { get; set; }
        public int? FacilityId { get; set; }
        public string? FacilityName { get; set; }

        public string BankAccount { get; set; }

        public bool? Status { get; set; }
        public List<string> ScheduleDays { get; set; } = new List<string>(); // Thêm danh sách ngày
    }
}
