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
    public class IndexModel : PageModel
    {
        private readonly MedicaiFacility.DataAccess.AppDbContext _context;

        public IndexModel(MedicaiFacility.DataAccess.AppDbContext context)
        {
            _context = context;
        }

        public IList<RatingsAndFeedback> RatingsAndFeedback { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RatingsAndFeedback = await _context.RatingsAndFeedbacks
                .Include(r => r.MedicalHistory).ToListAsync();
        }
    }
}
