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

        public MedicalFacility MedicalFacility { get; set; }

        public EditModel(IMedicalFacilityService medicalFacilityService)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _medicalFacilityService.UpdateMedicalFacility(MedicalFacility);
            return RedirectToPage("Index");

        }
    }
}
