using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService; // For IMedicalExpertService

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    public class DetailsModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;

        public DetailsModel(IMedicalExpertService medicalExpertService)
        {
            _medicalExpertService = medicalExpertService;
        }

        public MedicalExpert MedicalExpert { get; set; } = default!;
        public List<string> Schedule { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Invalid Expert ID.";
                return RedirectToPage("/MedicalExperts/Index");
            }

            try
            {
                var medicalExpert = await _medicalExpertService.GetByIdAsync(id.Value);

                if (medicalExpert == null)
                {
                    TempData["Error"] = $"Medical Expert with ID {id} not found.";
                    return RedirectToPage("/MedicalExperts/Index");
                }

                MedicalExpert = medicalExpert;
                Schedule = _medicalExpertService.GetScheduleByExpertId(id.Value);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while fetching the Medical Expert: {ex.Message}";
                return RedirectToPage("/MedicalExperts/Index");
            }
        }
    }
}