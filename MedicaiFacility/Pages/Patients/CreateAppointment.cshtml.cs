using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    [BindProperties]
    public class CreateAppointmentModel : PageModel
    {
        public Appointment Appointment { get; set; }
        public Patient Patient { get; set; }
        public int PatientId { get; set; }
        public MedicalExpert MedicalExpert { get; set; }
        public int TransactionId { get; set; }
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IMedicalExpertService _medicalExpertService;
        public CreateAppointmentModel(IAppointmentService appointmentService,IPatientService patientService,IMedicalExpertService medicalExpertService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _medicalExpertService = medicalExpertService;
        }
        public void OnGet(int? patientId,int? transactionId,int? expertId)
        {
          

            PatientId =(int) patientId;
            TransactionId = (int)transactionId;
            Patient = _patientService.getById(PatientId);
            MedicalExpert = _medicalExpertService.getById((int)expertId);
            var check = MedicalExpert.Facility.FacilityName;
        }
        public IActionResult OnPost()
        {
            var check = Appointment;
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
            ViewData["SuccessMessage"] = "Your appointment will be confirmed by a Medical Expert. Please wait!";
            return RedirectToPage("/Appointments/MyAppointments",new { pg =1, patientId = Appointment .PatientId});
        }
    }
}
