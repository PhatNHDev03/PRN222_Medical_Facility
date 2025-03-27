using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class IndexModel : PageModel
    {
        private readonly IRatingsAndFeedbackService _service;

        public IndexModel(IRatingsAndFeedbackService service)
        {
            _service = service;
        }

        public List<RatingsAndFeedback> RatingsAndFeedback { get; private set; } = new List<RatingsAndFeedback>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            RatingsAndFeedback = _service.GetAll().OrderByDescending(x=>x.FeedbackId).ToList();
            return Page();
        }
    }
}
