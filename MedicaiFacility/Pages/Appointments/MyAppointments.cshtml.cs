using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Appointments
{
    [BindProperties]
    public class MyAppointmentsModel : PageModel
    {
        public int PatientId { get; set; }
        public Pager Pager { get; set; }
        public List<AppoinmentViewModel> AppointmentViewModels { get; set; } = new List<AppoinmentViewModel>();
        private readonly IAppointmentService _appointmentService;
        private readonly ITransactionService _transactionService;
        private readonly ISystemBalanceService _systemBalanceService;
        public MyAppointmentsModel(IAppointmentService appointmentService, ITransactionService transactionService, ISystemBalanceService systemBalanceService)
        {
            _appointmentService = appointmentService;
            _transactionService = transactionService;
            _systemBalanceService = systemBalanceService;
        }

        public void OnGet(int pg = 1, int patientId = 0)
        {
            if (patientId == null)
            {
                RedirectToPage("/Error");
                return;
            }
            PatientId = patientId;
            int pageSize = 5;
            var (list, total) = _appointmentService.GetALlPagainationsByPatientId(pg, pageSize, (int)patientId);

            Pager = new Pager(total, pg, pageSize);

            // Sử dụng LINQ .Select() thay vì foreach
            AppointmentViewModels = list.Select(item => new AppoinmentViewModel
            {
                AppointmentId = item.AppointmentId,
                PatientId = item.PatientId,
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
            }).ToList();

        }



        public IActionResult OnPostCancelMyAppointment(int id, int pId)
        {
            var item = _appointmentService.GetById(id);
            if (item != null)
            {
                item.Status = "Cancelled";
                _appointmentService.Update(item);

                if (item.TransactionId.HasValue)
                {
                    var existingTransaction = _transactionService.GetById(item.TransactionId.Value);
                    if (existingTransaction != null)
                    {
                        Transaction refundTransaction = new Transaction
                        {
                            UserId = item.PatientId,
                            PaymentMethod = "Vn pay",
                            Amount = existingTransaction.Amount,
                            TransactionStatus = "success",
                            CreatedAt = DateTime.UtcNow,
                            UpdateAt = DateTime.UtcNow,
                            TransactionType = "RefundTransaction"
                        };

                        _transactionService.Create(refundTransaction);


                        _systemBalanceService.update(1, -existingTransaction.Amount);
                    }
                }
            }

            return RedirectToPage("/Patients/MyAppointment", new { pg = 1, patientId = pId });
        }
    }
}
