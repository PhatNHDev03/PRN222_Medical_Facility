using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
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
        ViewData["ExpertId"] = new SelectList(_context.Users, "UserId", "Email");
        ViewData["FacilityId"] = new SelectList(_context.MedicalFacilities, "FacilityId", "Address");
            return Page();
        }

        [BindProperty]
        public MedicalExpert MedicalExpert { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MedicalExperts.Add(MedicalExpert);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
