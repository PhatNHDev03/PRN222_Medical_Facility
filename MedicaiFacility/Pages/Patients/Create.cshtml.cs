using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IPatientService _patientService;

        public CreateModel(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }


        public InputModel Input { get; set; }
        public int UserId { get; set; }
        public class InputModel
        {
            public DateTime? DateOfBirth { get; set; }

            public string Gender { get; set; }

            public string MedicalHistory { get; set; }
        }

        public IActionResult OnGet(int  id)
        {
            // L?y UserId t? thông tin ??ng nh?p
            UserId = id;
          
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
               
            

                var patient = new Patient
                {
                    PatientId = UserId,
                    DateOfBirth = Input.DateOfBirth,
                    Gender = Input.Gender,
                    MedicalHistory = Input.MedicalHistory
                };

                _patientService.CreatePatient(patient);

                TempData["SuccessMessage"] = "Patient created successfully!";
                return RedirectToPage("/Users/Login");
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