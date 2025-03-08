using Azure;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace MedicaiFacility.RazorPage.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly IDepartmentService _departmentService;

        [BindProperty]
        public Department Department { get; set; }
        public DeleteModel(IDepartmentService departmentService)
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
            if (Department == null)
            {
                return NotFound();
            }

            try
            {
                _departmentService.DeleteDepartment(Department.DepartmentId);
                TempData["SuccessMessage"] = "Department deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete department: {ex.Message}";
                return Page();
            }
        }
    }
}
