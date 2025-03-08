using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Diseases
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IDiseaseService _diseaseService;
        private readonly IDepartmentService _departmentService;

        public Disease Disease { get; set; }
        public List<Department> Departments { get; set; }

        public CreateModel(IDiseaseService diseaseService, IDepartmentService departmentService)
        {
            _diseaseService = diseaseService;
            _departmentService = departmentService;
        }
        public IActionResult OnGet()
        {
            //Diseases = _diseaseService.GetAllDisease();
            Departments = _departmentService.GetAllDepartment();
            Disease = new Disease()
            {
                IsActive = true,
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }
            if (Disease.DepartmentId == 0 || Disease.DepartmentId == null)
            {
                return new JsonResult(new { success = false, message = "Please select a valid department." });
            }
            //Check có lay dc id k
            Console.WriteLine($"Received Disease: Name={Disease.DiseaseName}, Symptoms={Disease.Symptoms}, Description={Disease.Description}, DepartmentID={Disease.DepartmentId}");
            try
            {
                _diseaseService.AddDisease(Disease);
                return new JsonResult(new { success = true, message = "Disease created successfully!", redirectUrl = "/Diseases/Index" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.ToString());
                return new JsonResult(new { success = false, message = $"Error creating department: {ex.Message}" });
            }
        }

    }
}
