using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class DetailModel : PageModel
    {
        private readonly IRatingsAndFeedbackService _ratingsService;

        public DetailModel(IRatingsAndFeedbackService ratingsService)
        {
            _ratingsService = ratingsService;
        }

        public RatingsAndFeedback RatingsAndFeedback { get; set; } = default!;

        public IActionResult OnGet(int id) // Đổi async Task thành IActionResult vì không cần await
        {
            Console.WriteLine($"OnGetAsync called with intHis={id}");
          

            var ratingsAndFeedback = _ratingsService.FindById(id);
            if (ratingsAndFeedback == null)
            {
                Console.WriteLine("RatingsAndFeedback not found");
                return NotFound();
            }

            RatingsAndFeedback = ratingsAndFeedback;
            return Page();
        }

    }
}
