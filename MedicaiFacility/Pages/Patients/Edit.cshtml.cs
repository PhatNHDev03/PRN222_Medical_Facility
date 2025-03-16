using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    public class EditModel : PageModel
    {
        private readonly IPatientService _patientService;

        public EditModel(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        public class InputModel
        {
            [Required]
            public int PatientId { get; set; }

            [Required]
            [Display(Name = "Date of Birth")]
            public DateTime? DateOfBirth { get; set; }

            [Required]
            public string Gender { get; set; }

            [Display(Name = "Medical History")]
            public string MedicalHistory { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                var patient = _patientService.GetPatientById(id);
                if (patient == null)
                {
                    return NotFound();
                }

                Input = new InputModel
                {
                    PatientId = patient.PatientId,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    MedicalHistory = patient.MedicalHistory
                };

                // Tr? v? n?i dung HTML c?a trang ?? hi?n th? trong modal
                return Page();
            }
            catch
            {
                return NotFound();
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = "Validation failed: " + string.Join(", ", errors) });
            }

            var patient = new Patient
            {
                PatientId = Input.PatientId,
                DateOfBirth = Input.DateOfBirth,
                Gender = Input.Gender,
                MedicalHistory = Input.MedicalHistory
            };

            try
            {
                _patientService.UpdatePatient(patient);
                return new JsonResult(new { success = true, message = "Patient updated successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}