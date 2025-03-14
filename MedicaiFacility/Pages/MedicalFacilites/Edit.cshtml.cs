using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IDepartmentService _departmentService; 
        private readonly IFacilityDepartmentService _facilityDepartmentService; 

        public MedicalFacility MedicalFacility { get; set; }
        public List<Department> AvailableDepartments { get; set; }
        public List<int?> SelectedDepartmentIds { get; set; } 
        public EditModel(IMedicalFacilityService medicalFacilityService, IDepartmentService departmentService, IFacilityDepartmentService facilityDepartmentService)
        {
            _medicalFacilityService = medicalFacilityService;
            _departmentService = departmentService;
        }

        public IActionResult OnGet(int id)
        {
            MedicalFacility = _medicalFacilityService.FindById(id);
            if (MedicalFacility == null)
            {
                return NotFound();
            }

            // Fetch all available departments
            AvailableDepartments = _departmentService.GetAllDepartment();

            // Fetch the currently associated department IDs for this facility
            SelectedDepartmentIds = _medicalFacilityService.GetDepartmentIdsByFacilityId(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var nonNullableSelectedDepartmentIds = SelectedDepartmentIds?.Where(id => id.HasValue).Select(id => id.Value).ToList() ?? new List<int>();
            _medicalFacilityService.UpdateMedicalFacilityWithDepartments(MedicalFacility, nonNullableSelectedDepartmentIds);

            TempData["SuccessMessage"] = "Medical Facility updated successfully!";
            return RedirectToPage("Index");

        }
    }
}
