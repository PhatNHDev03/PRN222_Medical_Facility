using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.Search
{
    [BindProperties]
    public class SearchDoctorsModel : PageModel
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IDepartmentService _departmentService;
        private readonly IMedicalFacilityService _medicalFacilityService;

        public SearchDoctorsModel(
            IMedicalExpertService medicalExpertService,
            IDepartmentService departmentService,
            IMedicalFacilityService medicalFacilityService)
        {
            _medicalExpertService = medicalExpertService;
            _departmentService = departmentService;
            _medicalFacilityService = medicalFacilityService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedSpecialization { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedDepartment { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedFacilityId { get; set; }

        public List<DoctorViewModel> Doctors { get; set; }

        public List<SelectListItem> Specializations { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> MedicalFacilities { get; set; }

        public void OnGet(int pg = 1)
        {
            // Dropdown
            PopulateDropdowns();

            var medicalExperts = _medicalExpertService.GetAllMedicalExpert();
            var filter = medicalExperts.Where(m => string.IsNullOrEmpty(SearchTerm) ||
                             m.Specialization.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             (m.Department ?? "").Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             m.ExperienceYears.ToString().Trim().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             m.StartHour.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             m.EndHour.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             m.Facility.FacilityName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                               m.Facility.Address.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                             )
                             .OrderByDescending(m => m.ExpertId)
                             .ToList();

            if (!string.IsNullOrEmpty(SelectedSpecialization))
            {
                filter = filter
                    .Where(me => me.Specialization != null && me.Specialization.Equals(SelectedSpecialization, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(SelectedDepartment))
            {
                filter = filter
                    .Where(me => me.Department != null && me.Department.Equals(SelectedDepartment, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (SelectedFacilityId.HasValue && SelectedFacilityId.Value > 0)
            {
                filter = filter
                    .Where(me => me.FacilityId == SelectedFacilityId.Value)
                    .ToList();
            }

            // Pagination logic
            int pageSize = 3;
            TotalPages = (int)Math.Ceiling((double)filter.Count / pageSize);
            CurrentPage = pg;
            var paginationList = filter.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            // Map to DoctorViewModel
            Doctors = paginationList.Select(me => new DoctorViewModel
            {
                ExpertId = me.ExpertId,
                FullName = me.Expert?.FullName ?? "Unknown",
                Image = me.Expert?.Image ?? "https://cdn.medpro.vn/prod-partner/3ee52d40-60a4-492b-af06-f759cce2d5d2-doctormale.jpg",
                workHour = me.StartHour + "h" + " - " + me.EndHour + "h",
                Specialization = me.Specialization ?? "N/A",
                ExperienceYears = me.ExperienceYears,
                DepartmentName = me.Department ?? "N/A",
                FacilityName = me.Facility?.FacilityName ?? "N/A",
                Address = me.Facility?.Address ?? "N/A",
                PriceBooking = me.PriceBooking ?? 0,
            }).ToList();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new { SearchTerm, SelectedSpecialization, SelectedDepartment, SelectedFacilityId });
        }

        private void PopulateDropdowns()
        {
            // Populate Specializations
            var specializationList = new List<string>
            {
                "Senior specialist",
                "Specialist doctor",
                "General practitioner"
            };
            Specializations = specializationList
                .Select(s => new SelectListItem { Value = s, Text = s })
                .ToList();
            Specializations.Insert(0, new SelectListItem { Value = "", Text = "All Specializations" });

            // Populate Departments
            var departments = _departmentService.GetAllDepartment();
            Departments = departments
                .Select(d => new SelectListItem { Value = d.DepartmentName, Text = d.DepartmentName })
                .ToList();
            Departments.Insert(0, new SelectListItem { Value = "", Text = "All Departments" });

            // Populate Medical Facilities
            var facilities = _medicalFacilityService.GetAllMedicalFacility();
            MedicalFacilities = facilities
                .Select(f => new SelectListItem { Value = f.FacilityId.ToString(), Text = f.FacilityName })
                .ToList();
            MedicalFacilities.Insert(0, new SelectListItem { Value = "", Text = "All Facilities" });
        }
    }
}