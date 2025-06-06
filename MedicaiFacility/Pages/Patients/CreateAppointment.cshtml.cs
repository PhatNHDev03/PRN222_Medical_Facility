﻿using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.Hubs;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    [BindProperties]
    public class CreateAppointmentModel : PageModel
    {
        public Appointment Appointment { get; set; }
        public Patient Patient { get; set; }
        public int PatientId { get; set; }
        public MedicalExpert MedicalExpert { get; set; }
        public List<string> WorkingDays { get; set; }
        public List<string> BookedSlots { get; set; }
        public List<string> StartBookedSlot { get; set; }
        public List<string> EndBookedSlot { get; set; }
        public int TransactionId { get; set; }
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IMedicalExpertService _medicalExpertService;
        private readonly IHubContext<SignalRServer> _signalHub;
        public CreateAppointmentModel(IAppointmentService appointmentService,IPatientService patientService,IMedicalExpertService medicalExpertService, IHubContext<SignalRServer> signalHub)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _medicalExpertService = medicalExpertService;
            _signalHub = signalHub;
        }
        public void OnGet(int? patientId,int? transactionId,int? expertId)
        {
            PatientId =(int) patientId;
            TransactionId = (int)transactionId;
            Patient = _patientService.getById(PatientId);
            MedicalExpert = _medicalExpertService.getById((int)expertId);
            var check = MedicalExpert.StartHour;
            WorkingDays = MedicalExpert.MedicalExpertSchedules
                       .Select(x => x.DayOfWeek)
                       .ToList();
            // Lấy danh sách các StartDate đã được đặt
            BookedSlots = _appointmentService.GetAllByExpertId((int)expertId)
                            .Where(x => x.Status == "Confirmed" && x.EndDate.HasValue&&x.StartDate>=DateTime.Now)
                            .Select(x => "Start Date: "+ x.StartDate.ToString("yyyy-MM-ddTHH:mm") +" End Date: " + x.EndDate.Value.ToString("yyyy-MM-ddTHH:mm"))  // Format theo datetime-local
                            .ToList();
            StartBookedSlot = _appointmentService.GetAllByExpertId((int)expertId)
                .Where(x => x.Status == "Confirmed")
                .Select(x => x.StartDate.ToString("yyyy-MM-ddTHH:mm"))  // Format theo datetime-local
                .ToList();
            EndBookedSlot = _appointmentService.GetAllByExpertId((int)expertId)
               .Where(x => x.Status == "Confirmed"&&x.EndDate.HasValue)
               .Select(x => x.EndDate.Value.ToString("yyyy-MM-ddTHH:mm"))  // Format theo datetime-local
               .ToList();
        }
        public async Task<IActionResult> OnPost()
        {
           
            Appointment.Status = "Pending";
            Appointment.CreatedAt = DateTime.Now;
            Appointment.UpdatedAt = DateTime.Now;
            if (Appointment.StartDate > Appointment.EndDate)
            {
                ModelState.AddModelError("Appointment.StartDate", "Start date cannot be later than end date");
                ViewData["ErrorMessage"] = "Start date cannot be later than end date";

                // Re-populate the model to maintain the form data
                PatientId = (int)Appointment.PatientId;
                Patient = _patientService.getById(PatientId);
                MedicalExpert = _medicalExpertService.getById((int)Appointment.ExpertId);
                TransactionId = Appointment.TransactionId ?? 0;

                return Page();
            }
            _appointmentService.Create(Appointment);
            await _signalHub.Clients.All.SendAsync("ReceiveDeletedItem");
            ViewData["SuccessMessage"] = "Your appointment will be confirmed by a Medical Expert. Please wait!";
            return RedirectToPage("/Appointments/MyAppointments",new { pg =1, patientId = Appointment .PatientId});
        }
    }
}
