using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.FacilitityDepartments
{
    public class IndexModel : PageModel
    {
        public List<FacilityDepartment> FacilityDepartments { get; set; }
        public Pager Pager { get; set; }

        private readonly IFacilityDepartmentService _facilityDepartmentService;
        public IndexModel(IFacilityDepartmentService facilityDepartmentService)
        {
            _facilityDepartmentService = facilityDepartmentService;

        }
        public IActionResult OnGet(int pg = 1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            int pageSize = 5;
            var (facilityDepartmentsAll, totalItem) = _facilityDepartmentService.FindAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem, pg, pageSize);
            FacilityDepartments = facilityDepartmentsAll;
            return Page();
        }
    }
}