using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Transactions;

namespace MedicaiFacility.RazorPage.Pages.Appointments
{
    [BindProperties]
    public class DetailModel : PageModel
    {
        public AppoinmentViewModel AppointmentViewModel { get; set; }    
        private readonly IAppointmentService _appointmentService;
        public DetailModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public void OnGet(int? id)
        {
            var item = _appointmentService.GetById((int)id);
            AppointmentViewModel = new AppoinmentViewModel
            {
                AppointmentId = item.AppointmentId,
                PatientName = item.Patient.PatientNavigation.FullName,
                ExpertName = item.Expert.Expert.FullName,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                Status = item.Status,
                FacilityName = item.Facility.FacilityName,
                Address = item.Facility.Address,
                TransactionId = item.TransactionId, 
                PaymentMethod = item.Transaction.PaymentMethod,
                Amount = item.Transaction.Amount,
                TransactionStatus = item.Transaction.TransactionStatus
            };
        }
    }
}
