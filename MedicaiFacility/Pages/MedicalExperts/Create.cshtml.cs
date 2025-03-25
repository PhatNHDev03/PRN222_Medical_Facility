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
    [BindProperties]

    public class CreateModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IDepartmentService _departmentService;
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IUserService _userService;
        public CreateModel(
            IMedicalExpertService medicalExpertService,
            IDepartmentService departmentService,
            IMedicalFacilityService medicalFacilityService,
            IUserService userService
            )
        {
            _medicalExpertService = medicalExpertService ;
            _departmentService = departmentService ;
            _medicalFacilityService = medicalFacilityService ;
            _userService = userService ;
        }
        public MedicalExpert MedicalExpert { get; set; }
        public InputModel Input { get; set; }
        public List<SelectListItem> Specialization { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
        public List<SelectListItem> FacilityList { get; set; }

        public List<string> AvailableDays { get; set; }
        public int UserId { get; set; } 
        public class InputModel
        {
           

            public string[] SelectedDays { get; set; }
        }

        public IActionResult OnGet(int id)
        {
            UserId = _userService.FindById(id).UserId;
            LoadDropdowns();
            return Page();
        }
  

        public IActionResult OnPost()
        {

            var check = UserId;
            if (string.IsNullOrEmpty(MedicalExpert.Specialization))
            {
                ModelState.AddModelError("Input.Specialization", "Specialization is required.");
            }
            if (MedicalExpert.ExperienceYears <= 0)
            {
                ModelState.AddModelError("Input.ExperienceYears", "Experience Years must be a positive number.");
            }
            if (string.IsNullOrEmpty(MedicalExpert.Department))
            {
                ModelState.AddModelError("Input.Department", "Department is required.");
            }
            if (!MedicalExpert.FacilityId.HasValue)
            {
                ModelState.AddModelError("Input.FacilityId", "Facility is required.");
            }
            if (Input.SelectedDays == null || Input.SelectedDays.Length == 0)
            {
                ModelState.AddModelError("Input.SelectedDays", "Please select at least one day.");
            }

            

            try
            {
                var medicalExpert = new MedicalExpert
                {
                    ExpertId = UserId,
                    Specialization = MedicalExpert.Specialization,
                    ExperienceYears = MedicalExpert.ExperienceYears,
                    Department = MedicalExpert.Department,
                    StartHour = MedicalExpert.StartHour,
                    EndHour = MedicalExpert.EndHour,
                    PriceBooking = MedicalExpert.PriceBooking,
                    FacilityId = MedicalExpert.FacilityId
                };

                _medicalExpertService.CreateMedicalExpert(medicalExpert);

            

              
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
                        ExpertId = UserId,
                        DayOfWeek = day
                    };
                    _medicalExpertService.AddMedicalExpertSchedule(schedule);
                }

                TempData["SuccessMessage"] = "Medical Expert created successfully!";
                return RedirectToPage("/Users/Login");
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
            Specialization = new List<SelectListItem>
            {
                new SelectListItem { Value = "General practitioner", Text= "General practitioner" },
                new SelectListItem { Value = "Specialist doctor", Text= "Specialist doctor" },
                new SelectListItem { Value = "Senior specialist", Text= "Senior specialist" }
            };
            var departments = _departmentService.GetAllDepartment()
                .Where(d => d.IsActive == true).ToList();
            DepartmentList = departments.Select(x => new SelectListItem
            {
                Value = x.DepartmentId.ToString(), // Chuyển int thành string
                Text = x.DepartmentName
            }).ToList();



            var facilities = _medicalFacilityService.GetAllMedicalFacility()
                .Where(f => f.IsActive == true).ToList();
            FacilityList = facilities.Select(x => new SelectListItem
            {
                Value = x.FacilityId.ToString(), // Chuyển int thành string
                Text = x.FacilityName
            }).ToList();

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
