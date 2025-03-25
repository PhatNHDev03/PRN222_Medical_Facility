namespace MedicaiFacility.RazorPage.ViewModel
{
    public class RatingsAndFeedbackViewModel
    {
        public int FeedbackId { get; set; }

        public string? UserName { get; set; }
        public string? UserEmail { get; set; }

        public int Rating { get; set; } // Số sao đánh giá (1-5)

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
        public int? MedicalHistoryId { get; internal set; }
        public string Feedback { get; internal set; }
        public bool? IsActive { get; internal set; }
    }
}
