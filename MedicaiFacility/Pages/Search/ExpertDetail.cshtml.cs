using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.DataAccess;
using MedicaiFacility.RazorPage.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.Search
{
    public class ExpertDetailModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IUserService _userService;

        public ExpertDetailModel(IMedicalExpertService medicalExpertService, IUserService userService)   
        {
            _medicalExpertService = medicalExpertService;
            _userService = userService;
        }

        public DoctorViewModel Doctor { get; set; }
        public List<string> Schedule { get; set; }
        public List<FeedbackViewModel> Feedbacks { get; set; }

        public IActionResult OnGet(int expertId)
        {
            var medicalExpert = _medicalExpertService.getById(expertId);

            if (medicalExpert == null)
            {
                return NotFound();
            }

            Doctor = new DoctorViewModel
            {
                ExpertId = medicalExpert.ExpertId,
                FullName = medicalExpert.Expert?.FullName ?? "Unknown",
                Image = medicalExpert.Expert?.Image ?? "https://cdn.medpro.vn/prod-partner/3ee52d40-60a4-492b-af06-f759cce2d5d2-doctormale.jpg",
                Specialization = medicalExpert.Specialization ?? "N/A",
                ExperienceYears = medicalExpert.ExperienceYears,
                DepartmentName = medicalExpert.Department ?? "N/A",
                FacilityName = medicalExpert.Facility?.FacilityName ?? "N/A",
                Address = medicalExpert.Facility?.Address ?? "N/A",
                PriceBooking = medicalExpert.PriceBooking ?? 0,
                PhoneNumber = medicalExpert.Expert.PhoneNumber,
                Email = medicalExpert.Expert.Email
            };

            Schedule = _medicalExpertService.GetScheduleByExpertId(expertId);

            var feedbacks = _medicalExpertService.GetFeedbacksByExpertId(expertId);
            Feedbacks = feedbacks.Select(f => new FeedbackViewModel
            {
                UserName = GetUserNameFromFeedback(f), 
                CreatedAt = f.CreatedAt ?? DateTime.MinValue,
                Rating = f.Rating,
                Feedback = f.Feedback ?? "No feedback provided."
            }).ToList();

            return Page();
        }

        private string GetUserNameFromFeedback(RatingsAndFeedback feedback)
        {
            if (feedback?.MedicalHistory?.Appointment?.PatientId == null)
            {
                return "Anonymous";
            }

            var patient = _userService.GetUserByPatientId(feedback.MedicalHistory.Appointment.PatientId.Value);
            return patient?.FullName ?? "Anonymous";
        }
    }
}
