﻿using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMedicalExpertScheduleService _medicalExpertScheduleService; // Thêm service cho MedicalExpertSchedule
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public IndexModel(
            IUserService userService,
            IMedicalExpertScheduleService medicalExpertScheduleService)
        {
            _userService = userService;
            _medicalExpertScheduleService = medicalExpertScheduleService ;
        }

        public List<MedicalExpertViewModel> MedicalExperts { get; set; } = new List<MedicalExpertViewModel>();


        public void OnGet(int pg = 1)
        {
            CurrentPage = pg;
            var medicalExperts = _userService.GetAllUsers().Where(x => x.IsApprove == true && x.UserType == "MedicalExpert").ToList();
            MedicalExperts = medicalExperts
        .Select(expert =>
        {
            var viewModel = new MedicalExpertViewModel
            {
                ExpertId = expert.UserId,
                FullName = expert.FullName,
                Email = expert.Email,
                PhoneNumber = expert.PhoneNumber,
                Specialization = expert.MedicalExpert.Specialization,
                ExperienceYears = expert.MedicalExpert.ExperienceYears,
                Department = expert.MedicalExpert.Department,
                PriceBooking = expert.MedicalExpert.PriceBooking,
                FacilityName = expert.MedicalExpert.Facility.FacilityName,
                BankAccount = expert.BankAccount,
                Status = expert.Status
            };

            // Lấy danh sách ngày từ MedicalExpertSchedule
            var schedules = _medicalExpertScheduleService.GetSchedulesByExpertId(expert.MedicalExpert.ExpertId)
                .Where(s => s != null && !string.IsNullOrEmpty(s.DayOfWeek))
                .Select(s => s.DayOfWeek)
                .ToList();

            viewModel.ScheduleDays.AddRange(schedules);

            return viewModel;
        })
        .ToList();

            int pageSize = 4;
            TotalPages = (int)Math.Ceiling((double)MedicalExperts.Count / pageSize);
            CurrentPage = pg;
            MedicalExperts = MedicalExperts.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}