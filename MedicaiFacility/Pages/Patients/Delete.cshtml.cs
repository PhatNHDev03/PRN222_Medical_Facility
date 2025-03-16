using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    public class DeleteModel : PageModel
    {
        private readonly IPatientService _patientService;

        public DeleteModel(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        public class InputModel
        {
            [Required]
            public int Id { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Patient Patient { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                Patient = _patientService.GetPatientById(id);
                if (Patient == null)
                {
                    return NotFound();
                }

                Input = new InputModel { Id = id };
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnGet: {ex.Message}");
                return NotFound();
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Validation errors: " + string.Join(", ", errors));
                return new JsonResult(new { success = false, message = "Validation failed: " + string.Join(", ", errors) });
            }

            try
            {
                Console.WriteLine($"Attempting to delete patient with ID: {Input.Id}");
                _patientService.DeletePatient(Input.Id);
                Console.WriteLine($"Patient with ID {Input.Id} deleted successfully.");
                return new JsonResult(new { success = true, message = "Patient deleted successfully!" });
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }
                Console.WriteLine($"Error deleting patient: {errorMessage}");
                return new JsonResult(new { success = false, message = $"Failed to delete patient: {errorMessage}" });
            }
        }
    }
}