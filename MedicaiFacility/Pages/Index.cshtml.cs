using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Security.Cryptography.Pkcs;

namespace MedicaiFacility.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public List<MedicalFacilityViewModel> MedicalFacilityViewModels { get; set; } = new List<MedicalFacilityViewModel>();
        public string? SearchTerm  { get; set; }
        public string? Option { get; set; }
        public List<HealthArticle> HealthArticles { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
		public List<SelectListItem> DepartmentOptions { get; set; }
        public List<string>? SelectedDepartments { get; set; } = new();
   
        private readonly ILogger<IndexModel> _logger;
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IDepartmentService _departmentService;
        private readonly IHealthArticleService _healthArticleService;
        public IndexModel(ILogger<IndexModel> logger, IMedicalFacilityService medicalFacilityService, IDepartmentService departmentService, IHealthArticleService healthArticleService)
        {
            _logger = logger;
            _medicalFacilityService = medicalFacilityService;
            _departmentService = departmentService;
            _healthArticleService = healthArticleService;
        }

        public IActionResult OnGet(int pg=1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                return RedirectToPage("/MedicalExperts/Index");
            }
            HealthArticles = _healthArticleService.GetAll().Where(x=>x.IsActive==true).OrderByDescending(x=>x.ArticleId).ToList();
			int pageSize = 4;
			TotalPages = (int)Math.Ceiling((double)HealthArticles.Count / pageSize);
			CurrentPage = pg;
			HealthArticles = HealthArticles.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
			return Page();
        }


    

    }
}
