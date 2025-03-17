using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class CreateModel : PageModel
    {
        private readonly MedicaiFacility.DataAccess.AppDbContext _context;

        public CreateModel(MedicaiFacility.DataAccess.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["MedicalHistoryId"] = new SelectList(_context.MedicalHistories, "HistoryId", "HistoryId");
            return Page();
        }

        [BindProperty]
        public RatingsAndFeedback RatingsAndFeedback { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RatingsAndFeedbacks.Add(RatingsAndFeedback);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
