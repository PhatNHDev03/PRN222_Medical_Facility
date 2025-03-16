using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IMedicalExpertScheduleService _medicalExpertScheduleService; // Thêm service cho MedicalExpertSchedule

        public IndexModel(
            IMedicalExpertService medicalExpertService,
            IMedicalExpertScheduleService medicalExpertScheduleService)
        {
            _medicalExpertService = medicalExpertService ?? throw new ArgumentNullException(nameof(medicalExpertService));
            _medicalExpertScheduleService = medicalExpertScheduleService ?? throw new ArgumentNullException(nameof(medicalExpertScheduleService));
        }

        public List<MedicalExpertViewModel> MedicalExperts { get; set; } = new List<MedicalExpertViewModel>();

        public class MedicalExpertViewModel
        {
            public int ExpertId { get; set; }
            public string Specialization { get; set; }
            public int ExperienceYears { get; set; }
            public string Department { get; set; }
            public decimal? PriceBooking { get; set; }
            public int? FacilityId { get; set; }
            public List<string> ScheduleDays { get; set; } = new List<string>(); // Thêm danh sách ngày
        }

        public void OnGet()
        {
            var medicalExperts = _medicalExpertService.GetAllMedicalExperts() as IList<MedicalExpert> ?? new List<MedicalExpert>();

            foreach (var expert in medicalExperts)
            {
                var viewModel = new MedicalExpertViewModel
                {
                    ExpertId = expert.ExpertId,
                    Specialization = expert.Specialization,
                    ExperienceYears = expert.ExperienceYears,
                    Department = expert.Department,
                    PriceBooking = expert.PriceBooking,
                    FacilityId = expert.FacilityId
                };

                // L?y danh sách ngày t? MedicalExpertSchedule
                var schedules = _medicalExpertScheduleService.GetSchedulesByExpertId(expert.ExpertId)
                    .Where(s => s != null && !string.IsNullOrEmpty(s.DayOfWeek))
                    .Select(s => s.DayOfWeek)
                    .ToList();

                viewModel.ScheduleDays.AddRange(schedules);

                MedicalExperts.Add(viewModel);
            }
        }
    }
}