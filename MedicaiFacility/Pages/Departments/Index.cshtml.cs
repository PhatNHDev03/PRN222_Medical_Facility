using Azure;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Departments
{
    public class IndexModel : PageModel
    {
        public List<Department> Departments { get; set; }
        public Pager Pager { get; set; }

        private readonly IDepartmentService _departmentService;
        public IndexModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;

        }
        public void OnGet(int pg = 1)
        {
            int pageSize = 5;
            var (departmentsAll, totalItem) = _departmentService.FindAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem, pg, pageSize);
            Departments = departmentsAll;
        }
    }
}
