using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Diseases
{
    public class DeleteModel : PageModel
    {
        private readonly IDiseaseService _diseaseService;

        [BindProperty]
        public Disease Disease { get; set; }
        public DeleteModel(IDiseaseService diseaseService)
        {
            _diseaseService = diseaseService;
        }

        public IActionResult OnGet(int id)
        {
            Disease = _diseaseService.FindById(id);
            if (Disease == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (Disease == null)
            {
                return NotFound();
            }

            try
            {
                _diseaseService.DeleteDisease(Disease.DiseaseId);
                TempData["SuccessMessage"] = "Disease deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete Disease: {ex.Message}";
                return Page();
            }
        }
    }
}
