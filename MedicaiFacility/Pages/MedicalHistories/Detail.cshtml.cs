using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalHistories
{
    [BindProperties]
    public class DetailModel : PageModel
    {
        public MedicalHistoryViewModel MedicalHistoryViewModel { get; set; }
        private readonly IMedicalHistoryService _medicalHistoryService;
        public DetailModel(IMedicalHistoryService medicalHistoryService)
        {
            _medicalHistoryService = medicalHistoryService;
        }
        public void OnGet(int? id)
        {
            var item = _medicalHistoryService.GetById((int)id);
            MedicalHistoryViewModel = new MedicalHistoryViewModel
            {
                HistoryId = item.HistoryId,
                Description = item.Description,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                StartDate = item.Appointment.StartDate,
                EndDate = item.Appointment.EndDate,
                AppointmentId = item.Appointment.AppointmentId,
                AppointmentStatus = item.Appointment.Status,
                patientInfor = item.Appointment.Patient.PatientNavigation.FullName + " " + item.Appointment.Patient.PatientNavigation.PhoneNumber + " " + item.Appointment.Patient.PatientNavigation.Email,
                ExpertInfor = item.Appointment.Expert.Expert.FullName + " " + item.Appointment.Expert.Expert.PhoneNumber + " " + item.Appointment.Expert.Expert.Email
            };
        }
    }
}
