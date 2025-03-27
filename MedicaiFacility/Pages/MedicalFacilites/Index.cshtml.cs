using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IDepartmentService _departmentService;
        private readonly IFacilityDepartmentService _facilityDepartmentService;

        public List<MedicalFacility> MedicalFacilities { get; set; }
        public Dictionary<int, List<string>> FacilityDepartments { get; set; } // To store department names for each facility
        public Pager Pager { get; set; }
        public IndexModel(IMedicalFacilityService medicalFacilityService, IFacilityDepartmentService facilityDepartmentService, IDepartmentService departmentService)
        {
            _medicalFacilityService = medicalFacilityService;
            _departmentService = departmentService;
            _facilityDepartmentService = facilityDepartmentService;
        }
        public IActionResult OnGet(int pg = 1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            int pageSize = 5;
            var (medicalFacilities, facilityDepartments, totalItem) = _medicalFacilityService.FindAllWithDepartmentsAndPagination(pg, pageSize);

            Pager = new Pager(totalItem, pg, pageSize);
            MedicalFacilities = medicalFacilities;
            FacilityDepartments = facilityDepartments;
            return Page();
        }
    }
}
