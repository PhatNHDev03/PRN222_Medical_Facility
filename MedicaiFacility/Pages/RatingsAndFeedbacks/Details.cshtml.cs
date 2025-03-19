using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class DetailsModel : PageModel
    {
        private readonly IRatingsAndFeedbackService _ratingsService;

        public DetailsModel(IRatingsAndFeedbackService ratingsService)
        {
            _ratingsService = ratingsService;
        }

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
            return Page();
        }
    }
}
