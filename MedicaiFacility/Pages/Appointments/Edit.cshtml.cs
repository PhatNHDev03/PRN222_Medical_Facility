using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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
            if (Appointment.EndDate <= Appointment.StartDate) {
                TempData["ErrorMessage"] = "Please do not update the start date too close to the end date.";
                return RedirectToPage("/Appointments/MyAppointments", new { pg = 1 });
            }

            Appointment.UpdatedAt = DateTime.Now;
            _appointmentService.Update(Appointment);
           
            if (User.FindFirstValue(ClaimTypes.Role)!= "Admin")
            {
                TempData["SuccessMessage"] = "Update successfully";
                return RedirectToPage("/Appointments/MyAppointments",new {pg=1 }); 
            }
            return RedirectToPage("/Appointments/Index");
        }
    }
}
