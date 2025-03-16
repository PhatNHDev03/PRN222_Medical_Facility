using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    [Authorize(Roles = "MedicalExpert")]
    public class CreateModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IDepartmentService _departmentService;
        private readonly IMedicalFacilityService _medicalFacilityService;

        public CreateModel(
            IMedicalExpertService medicalExpertService,
            IDepartmentService departmentService,
            IMedicalFacilityService medicalFacilityService)
        {
            _medicalExpertService = medicalExpertService ?? throw new ArgumentNullException(nameof(medicalExpertService));
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            _medicalFacilityService = medicalFacilityService ?? throw new ArgumentNullException(nameof(medicalFacilityService));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList DepartmentList { get; set; }
        public SelectList FacilityList { get; set; }

        public List<string> AvailableDays { get; set; }

        public class InputModel
        {
            [Required]
            public string Specialization { get; set; }

            [Required]
            [Range(0, int.MaxValue, ErrorMessage = "Experience Years must be a positive number.")]
            public int ExperienceYears { get; set; }

            [Required]
            public string Department { get; set; }

            [Range(0.00, double.MaxValue, ErrorMessage = "Price Booking must be a positive number.")]
            public decimal? PriceBooking { get; set; }

            [Required]
            public int? FacilityId { get; set; }

            public string[] SelectedDays { get; set; }
        }

        public IActionResult OnGet()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                return RedirectToPage("/Users/Login");
            }

            Console.WriteLine($"Current UserId: {parsedUserId}");

            var departments = _departmentService.GetAllDepartment()
                .Where(d => d.IsActive == true);
            DepartmentList = new SelectList(departments, "DepartmentName", "DepartmentName");

            var facilities = _medicalFacilityService.GetAllMedicalFacility()
                .Where(f => f.IsActive == true);
            FacilityList = new SelectList(facilities, "FacilityId", "FacilityName");

            AvailableDays = new List<string>
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };
            Console.WriteLine($"AvailableDays count: {AvailableDays.Count}, Days: {string.Join(", ", AvailableDays)}");

            Input = new InputModel { FacilityId = null };
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                ModelState.AddModelError("", "Invalid user session.");
                Console.WriteLine("Invalid user session.");
                LoadDropdowns();
                return Page();
            }

            if (string.IsNullOrEmpty(Input.Specialization))
            {
                ModelState.AddModelError("Input.Specialization", "Specialization is required.");
            }
            if (Input.ExperienceYears <= 0)
            {
                ModelState.AddModelError("Input.ExperienceYears", "Experience Years must be a positive number.");
            }
            if (string.IsNullOrEmpty(Input.Department))
            {
                ModelState.AddModelError("Input.Department", "Department is required.");
            }
            if (!Input.FacilityId.HasValue)
            {
                ModelState.AddModelError("Input.FacilityId", "Facility is required.");
            }
            if (Input.SelectedDays == null || Input.SelectedDays.Length == 0)
            {
                ModelState.AddModelError("Input.SelectedDays", "Please select at least one day.");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid. Errors: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                LoadDropdowns();
                return Page();
            }

            try
            {
                Console.WriteLine($"Creating MedicalExpert for UserId: {parsedUserId}");
                var medicalExpert = new MedicalExpert
                {
                    ExpertId = parsedUserId,
                    Specialization = Input.Specialization,
                    ExperienceYears = Input.ExperienceYears,
                    Department = Input.Department,
                    PriceBooking = Input.PriceBooking,
                    FacilityId = Input.FacilityId
                };

                Console.WriteLine($"MedicalExpert data: Specialization={Input.Specialization}, ExperienceYears={Input.ExperienceYears}, Department={Input.Department}, PriceBooking={Input.PriceBooking}, FacilityId={Input.FacilityId}");
                _medicalExpertService.CreateMedicalExpert(medicalExpert);

                var createdExpert = _medicalExpertService.getById(parsedUserId);
                if (createdExpert == null)
                {
                    throw new Exception("Failed to create MedicalExpert in database.");
                }

                // Log giá tr? c?a Input.SelectedDays
                Console.WriteLine($"SelectedDays: {(Input.SelectedDays != null ? string.Join(", ", Input.SelectedDays) : "null")}");

                Console.WriteLine($"Adding schedules for ExpertId: {parsedUserId}");
                // L?c b? các giá tr? không h?p l? (chu?i r?ng)
                var validDays = Input.SelectedDays.Where(day => !string.IsNullOrWhiteSpace(day)).ToArray();
                if (validDays.Length == 0)
                {
                    ModelState.AddModelError("Input.SelectedDays", "Please select at least one valid day.");
                    LoadDropdowns();
                    return Page();
                }

                foreach (var day in validDays)
                {
                    var schedule = new MedicalExpertSchedule
                    {
                        ExpertId = parsedUserId,
                        DayOfWeek = day
                    };
                    Console.WriteLine($"Adding schedule: ExpertId={parsedUserId}, Day={day}");
                    _medicalExpertService.AddMedicalExpertSchedule(schedule);
                }

                TempData["SuccessMessage"] = "Medical Expert created successfully!";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnPost: {ex.Message} - StackTrace: {ex.StackTrace} - InnerException: {ex.InnerException?.Message}");
                ModelState.AddModelError("", $"Failed to create medical expert: {ex.Message}");
                LoadDropdowns();
                return Page();
            }
        }

        private void LoadDropdowns()
        {
            var departments = _departmentService.GetAllDepartment()
                .Where(d => d.IsActive == true);
            DepartmentList = new SelectList(departments, "DepartmentName", "DepartmentName");

            var facilities = _medicalFacilityService.GetAllMedicalFacility()
                .Where(f => f.IsActive == true);
            FacilityList = new SelectList(facilities, "FacilityId", "FacilityName");

            AvailableDays = new List<string>
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };
        }
    }
}