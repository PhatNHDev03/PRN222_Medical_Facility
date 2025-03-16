using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly IUserRepository _userRepository;

        public IndexModel(IPatientService patientService, IUserRepository userRepository)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public IList<Patient> Patients { get; set; }
        public IList<User> UserList { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;

        public void OnGet(int? page)
        {
            var allPatients = _patientService.GetAllPatients().ToList();
            int totalPatients = allPatients.Count;

            CurrentPage = page ?? 1;
            TotalPages = (int)Math.Ceiling(totalPatients / (double)PageSize);

            Patients = allPatients
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // L?y danh sách User ch?a có Patient ?? hi?n th? trong dropdown c?a modal Create
            UserList = _userRepository.GetAllUsers()
                .Where(u => u.Patient == null)
                .ToList();
        }

        public class InputModel
        {
            [Required]
            public int PatientId { get; set; }
            [Required]
            public DateTime? DateOfBirth { get; set; }
            [Required]
            public string Gender { get; set; }
            public string MedicalHistory { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnPostCreate()
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
                _patientService.CreatePatient(patient);
                return new JsonResult(new { success = true, message = "Patient created successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}