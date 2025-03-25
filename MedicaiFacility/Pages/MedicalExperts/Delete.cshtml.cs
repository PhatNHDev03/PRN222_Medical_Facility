using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    public class DeleteModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;

        [BindProperty]
        public MedicalExpert MedicalExpert { get; set; }
        public DeleteModel(IMedicalExpertService medicalExpertService)
        {
            _medicalExpertService = medicalExpertService;
        }

        public IActionResult OnGet(int id)
        {
            MedicalExpert = _medicalExpertService.getById(id);
            if (MedicalExpert == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (MedicalExpert == null)
            {
                return NotFound();
            }

            try
            {
                _medicalExpertService.DeleteMedicalExpert(MedicalExpert.ExpertId);
                TempData["SuccessMessage"] = "MedicalExpert deleted successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete MedicalExpert: {ex.Message}";
                return Page();
            }
        }
    }
}