using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.FacilityDepartments
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IFacilityDepartmentService _facilityDepartmentService;
        private readonly IDepartmentService _departmentService;
        private readonly IMedicalFacilityService _medicalFacilityService;

        public FacilityDepartment FacilityDepartment { get; set; }
        public List<Department> Departments { get; set; }
        public List<MedicalFacility> MedicalFacilities { get; set; }

        public CreateModel(IFacilityDepartmentService facilityDepartmentService, IDepartmentService departmentService, IMedicalFacilityService medicalFacilityService)
        {
            _facilityDepartmentService = facilityDepartmentService;
            _departmentService = departmentService;
            _medicalFacilityService = medicalFacilityService;
        }
        public IActionResult OnGet()
        {
            Departments = _departmentService.GetAllDepartment();
            MedicalFacilities = _medicalFacilityService.GetAllMedicalFacility();
            FacilityDepartment = new FacilityDepartment
            {
                CreatedAt = DateTime.Now, 
                Status = true
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            if (FacilityDepartment.DepartmentId == 0 || FacilityDepartment.DepartmentId == null)
            {
                return new JsonResult(new { success = false, message = "Please select a valid department." });
            }
            if (FacilityDepartment.FacilityId == 0 || FacilityDepartment.FacilityId == null)
            {
                return new JsonResult(new { success = false, message = "Please select a valid Facility." });
            }

            FacilityDepartment.CreatedAt = DateTime.Now;

            try
            {
                _facilityDepartmentService.AddFacilityDepartment(FacilityDepartment);
                return new JsonResult(new { success = true, message = "Facility Department created successfully!", redirectUrl = "/FacilityDepartment/Index" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.ToString());
                return new JsonResult(new { success = false, message = $"Error creating Facility Department: {ex.Message}" });
            }
        }

    }
}
