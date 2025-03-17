using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;

namespace MedicaiFacility.RazorPage.Pages.RatingsAndFeedbacks
{
    public class EditModel : PageModel
    {
        private readonly MedicaiFacility.DataAccess.AppDbContext _context;

        public EditModel(MedicaiFacility.DataAccess.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RatingsAndFeedback RatingsAndFeedback { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingsandfeedback =  await _context.RatingsAndFeedbacks.FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (ratingsandfeedback == null)
            {
                return NotFound();
            }
            RatingsAndFeedback = ratingsandfeedback;
           ViewData["MedicalHistoryId"] = new SelectList(_context.MedicalHistories, "HistoryId", "HistoryId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RatingsAndFeedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingsAndFeedbackExists(RatingsAndFeedback.FeedbackId))
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

        private bool RatingsAndFeedbackExists(int id)
        {
            return _context.RatingsAndFeedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
