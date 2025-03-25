using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    [BindProperties]
    public class CreateModel : PageModel
    {
       
        private readonly IRatingsAndFeedbackService _ratingsService;
        private readonly IMedicalHistoryService _medicalHistoryService;
        public int HisId { get; set; }
        public RatingsAndFeedback RatingsAndFeedback { get; set; } = default!;
        public CreateModel(IRatingsAndFeedbackService ratingsService, IMedicalHistoryService medicalHistoryService)
        {
            _ratingsService = ratingsService;
            _medicalHistoryService = medicalHistoryService;
        }

        public IActionResult OnGet(int intHis)
        {
            HisId = intHis;
            ViewData["MedicalHistoryId"] = new SelectList(_medicalHistoryService.GetAll(), "HistoryId", "HistoryId");
            return Page();
        }

      
   

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            RatingsAndFeedback.MedicalHistoryId = HisId;
            RatingsAndFeedback.IsActive = true;
            _ratingsService.Add(RatingsAndFeedback);

            return RedirectToPage("/MedicalHistories/MyMedicalHistory");
        }
    }
}
