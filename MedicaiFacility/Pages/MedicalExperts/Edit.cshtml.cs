using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    public class EditModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IMedicalExpertScheduleService _medicalExpertScheduleService;
        private readonly IUserService _userService;

        public EditModel(
            IMedicalExpertService medicalExpertService,
            IMedicalExpertScheduleService medicalExpertScheduleService,
            IUserService userService)
        {
            _medicalExpertService = medicalExpertService ?? throw new ArgumentNullException(nameof(medicalExpertService));
            _medicalExpertScheduleService = medicalExpertScheduleService ?? throw new ArgumentNullException(nameof(medicalExpertScheduleService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<string> AvailableDays { get; set; }

        public class InputModel
        {
            public int ExpertId { get; set; }

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

        public IActionResult OnGet(int id)
        {
            Console.WriteLine($"OnGet called for ExpertId: {id}");
            var medicalExpert = _medicalExpertService.getById(id);
            if (medicalExpert == null)
            {
                Console.WriteLine($"Medical Expert with ID {id} not found. Attempting to create...");
                var user = _userService.FindById(id);
                if (user == null)
                {
                    Console.WriteLine($"User with ID {id} not found. Creating default User.");
                    user = new User
                    {
                        UserId = id,
                        FullName = $"Default Expert {id}",
                        Email = $"expert{id}@example.com",
                        PhoneNumber = $"123456{id}",
                        Password = "default123",
                        UserType = "MedicalExpert",
                        Status = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _userService.SignUp(user);
                    Console.WriteLine($"Created default User with ID {id}");
                }

                medicalExpert = new MedicalExpert
                {
                    ExpertId = id,
                    Specialization = "Default Specialist",
                    ExperienceYears = 0,
                    Department = "Unknown",
                    PriceBooking = 0.00m,
                    FacilityId = 1
                };
                try
                {
                    _medicalExpertService.CreateMedicalExpert(medicalExpert);
                    medicalExpert = _medicalExpertService.getById(id);
                    if (medicalExpert == null)
                    {
                        Console.WriteLine($"Failed to create or retrieve Medical Expert with ID {id}.");
                        return NotFound();
                    }
                    Console.WriteLine($"Successfully created and retrieved Medical Expert with ID {id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating Medical Expert: {ex.Message} - InnerException: {ex.InnerException?.Message ?? "none"}");
                    return NotFound();
                }
            }

            Console.WriteLine($"Medical Expert found: {medicalExpert.ExpertId}, Expert: {medicalExpert.Expert?.UserId.ToString() ?? "null"}");
            Input = new InputModel
            {
                ExpertId = medicalExpert.ExpertId,
                Specialization = medicalExpert.Specialization,
                ExperienceYears = medicalExpert.ExperienceYears,
                Department = medicalExpert.Department,
                PriceBooking = medicalExpert.PriceBooking,
                FacilityId = medicalExpert.FacilityId,
                SelectedDays = _medicalExpertScheduleService.GetSchedulesByExpertId(id)
                    .Select(s => s.DayOfWeek)
                    .ToArray()
            };

            AvailableDays = new List<string>
            {
                "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                AvailableDays = new List<string>
                {
                    "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
                };
                return Page();
            }

            try
            {
                var medicalExpert = new MedicalExpert
                {
                    ExpertId = Input.ExpertId,
                    Specialization = Input.Specialization,
                    ExperienceYears = Input.ExperienceYears,
                    Department = Input.Department,
                    PriceBooking = Input.PriceBooking,
                    FacilityId = Input.FacilityId
                };

                Console.WriteLine($"Updating Medical Expert: {medicalExpert.ExpertId}, {medicalExpert.Specialization}");
                _medicalExpertService.UpdateMedicalExpert(medicalExpert);

                // Xóa l?ch trình c?
                Console.WriteLine($"Deleting schedules for ExpertId: {Input.ExpertId}");
                _medicalExpertService.DeleteSchedulesByExpertId(Input.ExpertId);

                // Thêm l?ch trình m?i
                var validDays = Input.SelectedDays?.Where(day => !string.IsNullOrWhiteSpace(day)).ToArray();
                if (validDays != null && validDays.Any())
                {
                    foreach (var day in validDays)
                    {
                        var schedule = new MedicalExpertSchedule
                        {
                            ExpertId = Input.ExpertId,
                            DayOfWeek = day
                        };
                        Console.WriteLine($"Adding schedule: ExpertId={schedule.ExpertId}, Day={schedule.DayOfWeek}");
                        _medicalExpertService.AddMedicalExpertSchedule(schedule);
                    }
                }

                TempData["SuccessMessage"] = "Medical Expert updated successfully!";
                return new JsonResult(new { success = true, message = "Medical Expert updated successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnPost: {ex.Message} - StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Failed to update medical expert: {ex.Message}");
                AvailableDays = new List<string>
                {
                    "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
                };
                return Page();
            }
        }
    }
}