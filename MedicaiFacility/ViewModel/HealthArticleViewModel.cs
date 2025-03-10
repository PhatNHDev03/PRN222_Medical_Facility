using System.ComponentModel.DataAnnotations;

namespace MedicaiFacility.RazorPage.ViewModel
{
    public class HealthArticleViewModel
    {
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Please provide health article title!")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "The title must be between 2 - 200 characters!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please provide health article content!")]
        [StringLength(5000, MinimumLength = 0, ErrorMessage = "The content must be between 50 - 5000 characters!")]
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
