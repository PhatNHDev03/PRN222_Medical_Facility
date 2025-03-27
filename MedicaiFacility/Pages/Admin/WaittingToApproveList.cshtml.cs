using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.Admin
{
    [BindProperties]
    public class WaittingToApproveListModel : PageModel
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        private readonly IUserService _userService;
        private readonly IMedicalExpertScheduleService _medicalExpertScheduleService;
       public List<MedicalExpertViewModel> MedicalExperts { get; set; }
        public WaittingToApproveListModel(IUserService userService, IMedicalExpertScheduleService medicalExpertScheduleService)
        {
            _userService = userService;
            _medicalExpertScheduleService = medicalExpertScheduleService;
        }
        public IActionResult OnGet(int pg=1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            CurrentPage = pg;
            var medicalExperts = _userService.GetAllUsers().Where(x=>x.IsApprove==false&&x.UserType== "MedicalExpert").ToList();
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
                FacilityName = expert.MedicalExpert.Facility.FacilityName
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

            int pageSize = 5;
            TotalPages = (int)Math.Ceiling((double)MedicalExperts.Count / pageSize);
            CurrentPage = pg;
            MedicalExperts = MedicalExperts.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            return Page();
        }

        public IActionResult OnPostApprove(int id) {

            var user = _userService.FindById(id);
            user.IsApprove = true;
            _userService.UpdateUser(user);
            return RedirectToPage("/Users/index");
        }

    }
}
