using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IDepartmentService _departmentService; 
        private readonly IFacilityDepartmentService _facilityDepartmentService; 
        public MedicalFacility MedicalFacility { get; set; }
        public List<Department> AvailableDepartments { get; set; } 
        public List<int> SelectedDepartmentIds { get; set; }
        public CreateModel(IMedicalFacilityService medicalFacilityService, IDepartmentService departmentService, IFacilityDepartmentService facilityDepartmentService)
        {
            _medicalFacilityService = medicalFacilityService;
            _departmentService = departmentService;
            _facilityDepartmentService = facilityDepartmentService;
        }
        public IActionResult OnGet()
        {
            MedicalFacility = new MedicalFacility
            {
                Verified = true,  
                IsActive = true    
            };
            AvailableDepartments = _departmentService.GetAllDepartment(); 
            SelectedDepartmentIds = new List<int>();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            if (SelectedDepartmentIds == null || !SelectedDepartmentIds.Any())
            {
                ModelState.AddModelError("SelectedDepartmentIds", "Please select at least one department.");
                return new JsonResult(new { success = false, message = "Please select at least one department." });
            }

            var validFacilityTypes = new[] { "Public Hospital", "Private Hospital", "Clinic" };
            if (string.IsNullOrEmpty(MedicalFacility.FacilityType) || !validFacilityTypes.Contains(MedicalFacility.FacilityType))
            {
                ModelState.AddModelError("MedicalFacility.FacilityType", "Invalid Facility Type. Must be one of: " + string.Join(", ", validFacilityTypes));
                return new JsonResult(new { success = false, message = "Invalid data! Invalid Facility Type." });
            }
;
            try
            {
                _medicalFacilityService.AddMedicalFacility(MedicalFacility);

                if (SelectedDepartmentIds != null && SelectedDepartmentIds.Any())
                {
                    foreach (var departmentId in SelectedDepartmentIds)
                    {
                        var facilityDepartment = new FacilityDepartment
                        {
                            FacilityId = MedicalFacility.FacilityId,
                            DepartmentId = departmentId
                        };
                        _facilityDepartmentService.AddFacilityDepartment(facilityDepartment); 
                    }
                }

                return new JsonResult(new { success = true, message = "Medical Facility created successfully!", redirectUrl = "/MedicalFacilities/Index" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.ToString());
                return new JsonResult(new { success = false, message = $"Error creating MedicalFacility: {ex.Message}" });
            }
        }

    }
}
