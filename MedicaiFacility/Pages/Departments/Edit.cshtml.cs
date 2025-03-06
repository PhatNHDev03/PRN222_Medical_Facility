using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Departments
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IDepartmentService _departmentService;

        public Department Department { get; set; }

        public EditModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public IActionResult OnGet(int id)
        {
            Department = _departmentService.FindById(id);
            if (Department == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _departmentService.UpdateDepartment(Department);
            return RedirectToPage("Index");

        }
    }
}
