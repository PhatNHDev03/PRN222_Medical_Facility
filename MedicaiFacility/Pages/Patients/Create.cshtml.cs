using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    [Authorize(Roles = "Patient")] // Ch? cho phép ng??i dùng có role "Patient"
    public class CreateModel : PageModel
    {
        private readonly IPatientService _patientService;

        public CreateModel(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public DateTime? DateOfBirth { get; set; }

            public string Gender { get; set; }

            public string MedicalHistory { get; set; }
        }

        public IActionResult OnGet()
        {
            // L?y UserId t? thông tin ??ng nh?p
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Users/Login");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
                {
                    ModelState.AddModelError("", "Invalid user session.");
                    return Page();
                }

                var patient = new Patient
                {
                    PatientId = parsedUserId,
                    DateOfBirth = Input.DateOfBirth,
                    Gender = Input.Gender,
                    MedicalHistory = Input.MedicalHistory
                };

                _patientService.CreatePatient(patient);

                TempData["SuccessMessage"] = "Patient created successfully!";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnPost: {ex.Message}");
                ModelState.AddModelError("", $"Failed to create patient: {ex.Message}");
                return Page();
            }
        }
    }
}