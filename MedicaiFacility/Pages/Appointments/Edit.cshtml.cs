using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Appointments
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public Appointment Appointment { get; set; }    
        private readonly IAppointmentService _appointmentService;
        public EditModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public void OnGet(int?id)
        {
            Appointment = _appointmentService.GetById((int)id);
        }
        public IActionResult OnPost() {
            var check = Appointment;
            Appointment.UpdatedAt = DateTime.Now;
            _appointmentService.Update(Appointment);
            return RedirectToPage("/Appointments/Index");
        }
    }
}
