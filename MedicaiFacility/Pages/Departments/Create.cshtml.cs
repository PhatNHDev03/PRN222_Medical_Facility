using Azure;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Departments
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IDepartmentService _departmentService;


        public Department Department { get; set; }
        public List<Department> Departments { get; set; }

        public CreateModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public IActionResult OnGet()
        {
            Department = new Department
            {
                IsActive = true    // Default value 
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            try
            {
                _departmentService.AddDepartment(Department);
                return new JsonResult(new { success = true, message = "Department created successfully!", redirectUrl = "/Departments/Index" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.ToString());
                return new JsonResult(new { success = false, message = $"Error creating department: {ex.Message}" });
            }
        }

    }
}
