using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class CreateModel : PageModel
    {
        private readonly IRatingsAndFeedbackService _ratingsService;
        private readonly IMedicalHistoryService _medicalHistoryService;

        public CreateModel(IRatingsAndFeedbackService ratingsService, IMedicalHistoryService medicalHistoryService)
        {
            _ratingsService = ratingsService;
            _medicalHistoryService = medicalHistoryService;
        }

        public IActionResult OnGet()
        {
            ViewData["MedicalHistoryId"] = new SelectList(_medicalHistoryService.GetAll(), "HistoryId", "HistoryId");
            return Page();
        }

        [BindProperty]
        public RatingsAndFeedback RatingsAndFeedback { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _ratingsService.Add(RatingsAndFeedback);

            return RedirectToPage("./Index");
        }
    }
}
