using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Appointments
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public Pager Pager { get; set; }
        public List<AppoinmentViewModel> AppointmentViewModels { get; set; } = new List<AppoinmentViewModel>();
        private readonly IAppointmentService _appointmentService;
        private readonly ITransactionService _transactionService;   
        public IndexModel(IAppointmentService appointmentService, ITransactionService transactionService)
        {
            _appointmentService = appointmentService;
            _transactionService = transactionService;
        }
        public void OnGet(int pg =1)
        {
            int pageSize = 5;
            var (list, total) = _appointmentService.GetALlPagainations(pg, pageSize);
            Pager = new Pager(total, pg, pageSize);
            foreach (var item in list) {
               
                AppointmentViewModels.Add(
                    new AppoinmentViewModel {
                        AppointmentId = item.AppointmentId,
                        PatientName = item.Patient.PatientNavigation.FullName,
                        ExpertName = item.Expert.Expert.FullName,
                        FacilityName = item.Facility.FacilityName,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Status = item.Status,
                        Address = item.Facility.Address,
                        PaymentMethod = item.Transaction.PaymentMethod,
                        Amount = item.Transaction.Amount,
                        TransactionStatus = item.Transaction.TransactionStatus
                    }
                    );
            }

        }
        public IActionResult OnPostDeleteById(int id) { 
            var item = _appointmentService.GetById(id);
            if (item != null) {
                item.Status = "IsDelete";
                _appointmentService.Update(item);
            }
            return RedirectToPage("/Appointments/Index");
        }
    }
}
