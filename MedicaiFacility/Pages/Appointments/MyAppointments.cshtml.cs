using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.Appointments
{
    [BindProperties]
    public class MyAppointmentsModel : PageModel
    {
        public int UserId { get; set; }
        public int ExpertId { get; set; }
        public Pager Pager { get; set; }
        public List<AppoinmentViewModel> AppointmentViewModels { get; set; } = new List<AppoinmentViewModel>();
        private readonly IAppointmentService _appointmentService;
        private readonly ITransactionService _transactionService;
        private readonly ISystemBalanceService _systemBalanceService;
        private readonly IMedicalHistoryService _medicalHistoryService;
        public MyAppointmentsModel(IAppointmentService appointmentService, ITransactionService transactionService, ISystemBalanceService systemBalanceService, IMedicalHistoryService medicalHistoryService)
        {
            _appointmentService = appointmentService;
            _transactionService = transactionService;
            _systemBalanceService = systemBalanceService;
            _medicalHistoryService = medicalHistoryService;
        }

        public void OnGet(int pg = 1)
        {
            PreLoadPage(pg);

		}
        public void OnPostCancelMyAppointment(int id)
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
                        var system = _systemBalanceService.GetBalance(1);
                        Transaction refundTransaction = new Transaction
                        {
                            Balance = system,
                            UserId = item.PatientId,
                            PaymentMethod = "Vn pay",
                            Amount = existingTransaction.Amount,
                            TransactionStatus = "success",
                            CreatedAt = DateTime.Now,
                            UpdateAt = DateTime.Now,
                            TransactionType = "RefundTransaction"
                        };

                        _transactionService.Create(refundTransaction);


                        _systemBalanceService.update(1, -existingTransaction.Amount);
                    }
                }
            }

            PreLoadPage(1);
    
        }

        public void OnPostConfirmedHandler(int id,string description)
        {
            var item = _appointmentService.GetById(id);
            if (item != null) {
                item.Status = "Confirmed";
                _appointmentService.Update(item);
               var medicalHistory = new MedicalHistory {
                AppointmentId = item.AppointmentId,
                   Description = description,
                   Status = "Pending",
                   Payed = false,
                   CreatedAt = DateTime.Now,
                   UpdatedAt = DateTime.Now
               };
               _medicalHistoryService.Create(medicalHistory);
            }
            PreLoadPage(1);
        
        }
        private void PreLoadPage(int pg){
            var list = new List<Appointment>();
            var total = 0;
            int pageSize = 5;
            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            (list, total) = (User.FindFirstValue(ClaimTypes.Role) == "Patient") ? _appointmentService.GetALlPagainationsByPatientId(pg, pageSize, UserId)
                : _appointmentService.GetALlPagainationsByExpertId(pg,pageSize,UserId);
			Pager = new Pager(total, pg, pageSize);

			// Sử dụng LINQ .Select() thay vì foreach
			AppointmentViewModels = list.Select(item => new AppoinmentViewModel
			{
				AppointmentId = item.AppointmentId,
				PatientId = item.PatientId,
				PatientName = item.Patient.PatientNavigation.FullName,
				ExpertDepartment = item.Expert.Department,
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
    
    }
}
