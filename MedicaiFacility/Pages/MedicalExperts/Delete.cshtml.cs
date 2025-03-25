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
        private readonly IUserService _userService;

        [BindProperty]
        public User MedicalExpert { get; set; }
        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet(int id)
        {
            MedicalExpert = _userService.FindById(id);
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
                var existing = _userService.FindById(MedicalExpert.UserId);
                existing.Status = false;
                _userService.UpdateUser(existing);
                TempData["SuccessMessage"] = "MedicalExpert deleted successfully!";
                return RedirectToPage("/MedicalExperts/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete MedicalExpert: {ex.Message}";
                return Page();
            }
        }
    }
}