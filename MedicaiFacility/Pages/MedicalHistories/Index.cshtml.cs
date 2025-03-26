using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Composition.Convention;

namespace MedicaiFacility.RazorPage.Pages.MedicalHistories
{
    [BindProperties]
    public class IndexModel : PageModel
    {

        public List<MedicalHistoryViewModel> MedicalHistoryViewModels { get; set; } = new List<MedicalHistoryViewModel>();
        public Pager Pager { get; set; }
        public decimal TotalBalance { get; set; }
        private readonly IMedicalHistoryService _medicalHistoryService;
        private readonly ITransactionService _transactionService;
        private readonly ISystemBalanceService _systemBalanceService;
        public IndexModel(IMedicalHistoryService medicalHistoryService, ITransactionService transactionService, ISystemBalanceService systemBalanceService)
        {
            _medicalHistoryService = medicalHistoryService;
            _transactionService = transactionService;
            _systemBalanceService = systemBalanceService;
        }
        public void OnGet(int pg=1)
        {
            TotalBalance = _systemBalanceService.GetBalance(1).TotalBalance;
            int pageSize = 5;
            var (listByPagination, total) = _medicalHistoryService.GetALlPagainations(pg,pageSize);
         
            Pager = new Pager(total, pg, pageSize);
            var checkPage =Pager.TotalPages;
            foreach (var item in listByPagination) {
                MedicalHistoryViewModels.Add(new MedicalHistoryViewModel {
                    HistoryId = item.HistoryId,
                    Description = item.Description,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    StartDate = item.Appointment.StartDate,
                    EndDate = item.Appointment.EndDate,
                    AppointmentStatus = item.Appointment.Status,
                    patientInfor= item.Appointment.Patient.PatientNavigation.FullName + " "+item.Appointment.Patient.PatientNavigation.PhoneNumber+" "+ item.Appointment.Patient.PatientNavigation.Email,
                    ExpertInfor = item.Appointment.Expert.Expert.FullName+ " " + item.Appointment.Expert.Expert.PhoneNumber+" "+ item.Appointment.Expert.Expert.Email,
                    Pay =(bool)item.Payed,
                    amount = item.Appointment.Transaction.Amount
                });

               
            }

        }
        public IActionResult OnPostDeleteById(int id) { 
            var item = _medicalHistoryService.GetById(id);
            if (item != null) {
                item.Status = "IsDeleted";
                _medicalHistoryService.Update(item);
            }
            return RedirectToPage("/MedicalHistories/Index");
        }
        public IActionResult OnGetPayOut(int id)
        {
            var item = _medicalHistoryService.GetById(id);
            if (item != null)
            {
                item.Payed = true;
                _medicalHistoryService.Update(item);
                var transactionPayed = _transactionService.GetById((int)item.Appointment.TransactionId);
                var system = _systemBalanceService.GetBalance(1);
                var transaction = new Transaction
                {
                    BalanceId = 1,
                    UserId = item.Appointment.ExpertId,
                    PaymentMethod = "Credit Card",
                    Amount = transactionPayed.Amount,
                    TransactionStatus = "Success",
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    TransactionType = "PayoutTransaction"
                };
                _transactionService.Create(transaction);
                // luu amount vao system balance ()
                _systemBalanceService.update(1, -transactionPayed.Amount);

            }
            return RedirectToPage("/MedicalHistories/Index");
            }
    }
}
