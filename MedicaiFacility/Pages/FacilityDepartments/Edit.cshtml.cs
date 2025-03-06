using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.FacilityDepartments
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IFacilityDepartmentService _facilityDepartmentService;
        private readonly IDepartmentService _departmentService;
        private readonly IMedicalFacilityService _medicalFacilityService;

        public FacilityDepartment FacilityDepartment { get; set; }
        public List<Department> Departments { get; set; }
        public List<MedicalFacility> MedicalFacilities { get; set; }

        public EditModel(IFacilityDepartmentService facilityDepartmentService, IDepartmentService departmentService, IMedicalFacilityService medicalFacilityService)
        {
            _facilityDepartmentService = facilityDepartmentService;
            _departmentService = departmentService;
            _medicalFacilityService = medicalFacilityService;
        }

        public IActionResult OnGet(int id)
        {
            Departments = _departmentService.GetAllDepartment();
            MedicalFacilities = _medicalFacilityService.GetAllMedicalFacility();
            FacilityDepartment = _facilityDepartmentService.FindById(id);
            if (FacilityDepartment == null)
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
            _facilityDepartmentService.UpdateFacilityDepartment(FacilityDepartment);
            return RedirectToPage("Index");

        }
    }
}
