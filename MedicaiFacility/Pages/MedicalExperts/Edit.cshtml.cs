using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    public class EditModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;

        public EditModel(
            IMedicalExpertService medicalExpertService,
            IMedicalFacilityService medicalFacilityService,
            IDepartmentService departmentService,
            IUserService userService)
        {
            _medicalExpertService = medicalExpertService;
            _medicalFacilityService = medicalFacilityService;
            _departmentService = departmentService;
            _userService = userService;
        }

        [BindProperty]
        public EditInputModel Input { get; set; } = new EditInputModel();

        public class EditInputModel
        {
            // User fields
            public int UserId { get; set; }

            [Required(ErrorMessage = "Full Name is required.")]
            [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "Invalid phone number format.")]
            [StringLength(15, ErrorMessage = "Phone Number cannot be longer than 15 characters.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "User Type is required.")]
            public string UserType { get; set; }

            [StringLength(50, ErrorMessage = "Bank Account cannot be longer than 50 characters.")]
            public string? BankAccount { get; set; }

            [Required(ErrorMessage = "Status is required.")]
            public bool? Status { get; set; }

            // MedicalExpert fields
            public int ExpertId { get; set; }

            [Required(ErrorMessage = "Specialization is required.")]
            public string Specialization { get; set; }

            [Required(ErrorMessage = "Experience Years is required.")]
            [Range(0, 100, ErrorMessage = "Experience Years must be between 0 and 100.")]
            public int ExperienceYears { get; set; }

            [Required(ErrorMessage = "Department is required.")]
            public string Department { get; set; }

            [Required(ErrorMessage = "Price Booking is required.")]
            [Range(0, 1000000000, ErrorMessage = "Price Booking must be greater than 0.")]
            public decimal PriceBooking { get; set; }

            [Required(ErrorMessage = "Start Hour is required.")]
            [Range(0, 24, ErrorMessage = "Start Hour must be from 0 to 24.")]
            public int StartHour { get; set; }

            [Required(ErrorMessage = "End Hour is required.")]
            [Range(0, 24, ErrorMessage = "End Hour must be from 0 to 24.")]
            public int EndHour { get; set; }

            [Required(ErrorMessage = "Facility is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Facility.")]
            public int FacilityId { get; set; }
        }

        public List<MedicalFacility> MedicaiFacilities { get; set; }
        public List<Department> Departments { get; set; }
        public SelectList Specializations { get; set; }
        public List<string> DaysOfWeek { get; set; } = new List<string>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };
        public List<string> SelectedDays { get; set; } = new List<string>();
        [BindProperty]
        public List<string> SelectedDaysFromForm { get; set; } = new List<string>();

        public List<SelectListItem> UserTypeOptions { get; set; }
        public List<SelectListItem> StatusOptions { get; set; }
        public List<SelectListItem> HourOptions { get; set; } // New property for hour dropdowns

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalExpert = await _medicalExpertService.GetByIdAsync(id.Value);
            if (medicalExpert == null)
            {
                return NotFound();
            }

            var user = _userService.FindById(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            // Populate the input model with data from both User and MedicalExpert
            Input = new EditInputModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                BankAccount = user.BankAccount,
                Status = user.Status,
                ExpertId = medicalExpert.ExpertId,
                Specialization = medicalExpert.Specialization,
                ExperienceYears = medicalExpert.ExperienceYears,
                Department = medicalExpert.Department,
                StartHour = medicalExpert.StartHour ?? default(int),
                EndHour = medicalExpert.EndHour ?? default(int),
                PriceBooking = medicalExpert.PriceBooking ?? default(decimal),
                FacilityId = medicalExpert.FacilityId ?? default(int)
            };

            // Populate dropdowns
            MedicaiFacilities = _medicalFacilityService.GetAllMedicalFacility();
            Departments = _departmentService.GetAllDepartment();
            var specializationList = new List<string>
            {
                "Senior specialist",
                "Specialist doctor",
                "General practitioner"
            };
            Specializations = new SelectList(specializationList, Input.Specialization);

            UserTypeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Patient", Text = "Patient" },
                new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
            };

            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Active" },
                new SelectListItem { Value = "false", Text = "Inactive" }
            };

            // Populate hour dropdown (0 to 24)
            HourOptions = new List<SelectListItem>();
            for (int i = 0; i <= 24; i++)
            {
                HourOptions.Add(new SelectListItem { Value = i.ToString(), Text = $"{i}:00" });
            }

            SelectedDays = _medicalExpertService.GetScheduleByExpertId(id.Value) ?? new List<string>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                MedicaiFacilities = _medicalFacilityService.GetAllMedicalFacility();
                Departments = _departmentService.GetAllDepartment();
                var specializationList = new List<string>
                {
                    "Senior specialist",
                    "Specialist doctor",
                    "General practitioner"
                };
                Specializations = new SelectList(specializationList, Input.Specialization);

                UserTypeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                    new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
                };

                StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "true", Text = "Active" },
                    new SelectListItem { Value = "false", Text = "Inactive" }
                };

                // Repopulate hour dropdown
                HourOptions = new List<SelectListItem>();
                for (int i = 0; i <= 24; i++)
                {
                    HourOptions.Add(new SelectListItem { Value = i.ToString(), Text = $"{i}:00" });
                }

                SelectedDays = SelectedDaysFromForm ?? new List<string>();
                return Page();
            }

            // Custom validation: Ensure EndHour is greater than StartHour
            if (Input.EndHour <= Input.StartHour)
            {
                ModelState.AddModelError("Input.EndHour", "End Hour must be greater than Start Hour.");
                return await ReloadDropdownsAndReturnPage();
            }

            // Custom validation: Ensure at least one day is selected for the schedule
            if (SelectedDaysFromForm == null || !SelectedDaysFromForm.Any())
            {
                ModelState.AddModelError("SelectedDaysFromForm", "Please select at least one available day for the schedule.");
                return await ReloadDropdownsAndReturnPage();
            }

            try
            {
                // Fetch the existing User and MedicalExpert
                var user = _userService.FindById(Input.UserId);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return await ReloadDropdownsAndReturnPage();
                }

                var medicalExpert = await _medicalExpertService.GetByIdAsync(Input.ExpertId);
                if (medicalExpert == null)
                {
                    ModelState.AddModelError(string.Empty, "Medical expert not found.");
                    return await ReloadDropdownsAndReturnPage();
                }

                // Check if the facility is active when activating the expert
                if (Input.Status == true && medicalExpert.Facility != null && medicalExpert.Facility.IsActive == false)
                {
                    ModelState.AddModelError(string.Empty, "Unable to activate specialist because the specialist's medical facility is inactive!");
                    return await ReloadDropdownsAndReturnPage();
                }

                // Update User properties
                user.FullName = Input.FullName;
                user.Email = Input.Email;
                user.PhoneNumber = Input.PhoneNumber;
                user.UserType = Input.UserType;
                user.BankAccount = Input.BankAccount;
                user.Status = Input.Status;
                user.UpdatedAt = DateTime.Now;

                // Update MedicalExpert properties
                medicalExpert.Specialization = Input.Specialization;
                medicalExpert.ExperienceYears = Input.ExperienceYears;
                medicalExpert.Department = Input.Department;
                medicalExpert.PriceBooking = Input.PriceBooking;
                medicalExpert.FacilityId = Input.FacilityId;
                medicalExpert.StartHour = Input.StartHour; // Update StartHour
                medicalExpert.EndHour = Input.EndHour;     // Update EndHour

                // Update the schedule
                await _medicalExpertService.UpdateScheduleAsync(medicalExpert.ExpertId, SelectedDaysFromForm);

                // Save changes
                _userService.UpdateUser(user);
                _medicalExpertService.UpdateMedicalExpert(medicalExpert);

                TempData["SuccessMessage"] = "Medical Expert updated successfully!";
                return RedirectToPage("/MedicalExperts/Details", new { id = Input.ExpertId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while saving changes: {ex.Message}");
                return await ReloadDropdownsAndReturnPage();
            }
        }

        private async Task<IActionResult> ReloadDropdownsAndReturnPage()
        {
            MedicaiFacilities = _medicalFacilityService.GetAllMedicalFacility();
            Departments = _departmentService.GetAllDepartment();
            var specializationList = new List<string>
            {
                "Senior specialist",
                "Specialist doctor",
                "General practitioner"
            };
            Specializations = new SelectList(specializationList, Input.Specialization);

            UserTypeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Patient", Text = "Patient" },
                new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
            };

            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Active" },
                new SelectListItem { Value = "false", Text = "Inactive" }
            };

            // Repopulate hour dropdown
            HourOptions = new List<SelectListItem>();
            for (int i = 0; i <= 24; i++)
            {
                HourOptions.Add(new SelectListItem { Value = i.ToString(), Text = $"{i}:00" });
            }

            SelectedDays = _medicalExpertService.GetScheduleByExpertId(Input.ExpertId) ?? new List<string>();
            return Page();
        }
    }
}