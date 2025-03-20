using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class EditModel : PageModel
    {
        private readonly IRatingsAndFeedbackService _ratingsService;
        private readonly IMedicalHistoryService _medicalHistoryService;

        public EditModel(IRatingsAndFeedbackService ratingsService, IMedicalHistoryService medicalHistoryService)
        {
            _ratingsService = ratingsService;
            _medicalHistoryService = medicalHistoryService;
        }

        [BindProperty]
        public RatingsAndFeedback RatingsAndFeedback { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingsAndFeedback = _ratingsService.FindById(id.Value);
            if (ratingsAndFeedback == null)
            {
                return NotFound();
            }

            RatingsAndFeedback = ratingsAndFeedback;
            ViewData["MedicalHistoryId"] = new SelectList(_medicalHistoryService.GetAll(), "HistoryId", "HistoryId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _ratingsService.Udpate(RatingsAndFeedback);
            }
            catch (Exception)
            {
                if (!_ratingsService.FindById(RatingsAndFeedback.FeedbackId).Equals(null))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
