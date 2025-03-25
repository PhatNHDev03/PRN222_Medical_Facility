namespace MedicaiFacility.RazorPage.ViewModel
{
    public class FeedbackViewModel
    {
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
    }
}
