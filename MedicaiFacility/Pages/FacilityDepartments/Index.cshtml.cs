using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public void OnGet(int pg = 1)
        {
            int pageSize = 5;
            var (facilityDepartmentsAll, totalItem) = _facilityDepartmentService.FindAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem, pg, pageSize);
            FacilityDepartments = facilityDepartmentsAll;
        }
    }
}