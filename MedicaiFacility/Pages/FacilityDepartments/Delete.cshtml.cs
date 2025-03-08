using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.FacilityDepartments
{
    public class DeleteModel : PageModel
    {
        private readonly IFacilityDepartmentService _facilityDepartmentService;

        [BindProperty]
        public FacilityDepartment FacilityDepartment { get; set; }
        public DeleteModel(IFacilityDepartmentService facilityDepartmentService)
        {
            _facilityDepartmentService = facilityDepartmentService;
        }

        public IActionResult OnGet(int id)
        {
            FacilityDepartment = _facilityDepartmentService.FindById(id);
            if (FacilityDepartment == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (FacilityDepartment == null)
            {
                return NotFound();
            }

            try
            {
                _facilityDepartmentService.DeleteFacilityDepartment(FacilityDepartment.FacilityDepartmentId);
                TempData["SuccessMessage"] = "Facility Department deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete Facility Department: {ex.Message}";
                return Page();
            }
        }
    }
}
