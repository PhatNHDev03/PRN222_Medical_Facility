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

        public List<SelectListItem> DepartmentOptions { get; set; }
        public List<string>? SelectedDepartments { get; set; } = new();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        private readonly ILogger<IndexModel> _logger;
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IDepartmentService _departmentService;
        public IndexModel(ILogger<IndexModel> logger, IMedicalFacilityService medicalFacilityService, IDepartmentService departmentService)
        {
            _logger = logger;
            _medicalFacilityService = medicalFacilityService;
            _departmentService = departmentService;
        }

        public IActionResult OnGet()
        {
            if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                return RedirectToPage("/MedicalExperts/Index");
            }
            return Page();
        }
    }
}
