using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.HealthArticles
{
    [BindProperties]
    public class EditModel : PageModel
    {
		public HealthArticle HealthArticle { get; set; }
		private readonly IHealthArticleService _healthArticleService;

		public EditModel(IHealthArticleService healthArticleService)
        {
            _healthArticleService = healthArticleService;
        }

       

        // OnGetAsync để load dữ liệu bài viết
        public IActionResult OnGetAsync(int id)
        {
             HealthArticle = _healthArticleService.FindById(id);

      

            return Page();
        }


        public IActionResult OnPostAsync()
        {
            // Lấy bài viết hiện tại từ cơ sở dữ liệu
            var existingArticle = _healthArticleService.FindById(HealthArticle.ArticleId);

            if (existingArticle == null)
            {
                return NotFound();
            }

            // Map dữ liệu từ HealthArticleBO sang đối tượng Entity, chỉ cập nhật Title và Content
            existingArticle.Title = HealthArticle.Title;
            existingArticle.Content = HealthArticle.Content;
			existingArticle.IsActive = HealthArticle.IsActive;
			// Giữ lại các trường khác như CreatedAt và AuthorName không thay đổi
			// (những trường này không cần thiết phải cập nhật lại)

			// Cập nhật bài viết trong cơ sở dữ liệu
			_healthArticleService.Udpate(existingArticle);   

            // Quay lại trang danh sách sau khi cập nhật
            return RedirectToPage("/HealthArticles/Index");
        }
    }
}
