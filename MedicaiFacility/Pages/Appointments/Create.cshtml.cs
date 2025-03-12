using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Appointments
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        public Appointment Appointment { get; set; }
        public int PatientId { get; set; }
        public int TransactionId { get; set; }
        private readonly IAppointmentService _appointmentService;

        public CreateModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost() {
            var check = Appointment;
           
            Appointment.CreatedAt = DateTime.Now;
            Appointment.UpdatedAt = DateTime.Now;
            _appointmentService.Create(Appointment);
            return RedirectToPage("/Appointments/Index");
        }
    }
}
