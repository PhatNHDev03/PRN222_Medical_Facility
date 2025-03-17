using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class DetailsModel : PageModel
    {
        private readonly MedicaiFacility.DataAccess.AppDbContext _context;

        public DetailsModel(MedicaiFacility.DataAccess.AppDbContext context)
        {
            _context = context;
        }

        public RatingsAndFeedback RatingsAndFeedback { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingsandfeedback = await _context.RatingsAndFeedbacks.FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (ratingsandfeedback == null)
            {
                return NotFound();
            }
            else
            {
                RatingsAndFeedback = ratingsandfeedback;
            }
            return Page();
        }
    }
}
