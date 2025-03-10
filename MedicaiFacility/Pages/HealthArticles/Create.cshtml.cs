using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.HealthArticles
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        public HealthArticle HealthArticle { get; set; }
        private readonly IHealthArticleService _healthArticleService;
        public CreateModel(IHealthArticleService healthArticleService)
        {
            _healthArticleService = healthArticleService;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost() {
            var check = HealthArticle;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            var userIdparse = userId.Value;

			HealthArticle.AuthorId = int.Parse(userId.Value);

			HealthArticle.IsActive = true;
            _healthArticleService.Save(HealthArticle);
            return RedirectToPage("/HealthArticles/Index");

		}
    }
}
