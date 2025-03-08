using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    public class DeleteModel : PageModel
    {
        private readonly IMedicalFacilityService _medicalFacilityService;

        [BindProperty]
        public MedicalFacility MedicalFacility { get; set; }
        public DeleteModel(IMedicalFacilityService medicalFacilityService)
        {
            _medicalFacilityService = medicalFacilityService;
        }

        public IActionResult OnGet(int id)
        {
            MedicalFacility = _medicalFacilityService.FindById(id);
            if (MedicalFacility == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (MedicalFacility == null)
            {
                return NotFound();
            }

            try
            {
                _medicalFacilityService.DeleteMedicalFacility(MedicalFacility.FacilityId);
                TempData["SuccessMessage"] = "Medical Facility deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete Medical Facility: {ex.Message}";
                return Page();
            }
        }
    }
}
