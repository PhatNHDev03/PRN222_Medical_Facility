using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.Hubs;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.MedicalHistories
{
    public class MyMedicalHistoryModel : PageModel
    {
        public List<MedicalHistoryViewModel> MedicalHistoryViewModels { get; set; } = new List<MedicalHistoryViewModel>();
        public Pager Pager { get; set; }
        private readonly IMedicalHistoryService _medicalHistoryService;
        private readonly ISystemBalanceService _systemBalanceService;
        private readonly ITransactionService _transactionService;
        private readonly IHealthRecordService _healthRecordService;
        private readonly IRatingsAndFeedbackService _ratingsAndFeedbackService;
        private readonly IHubContext<SignalRServer> _signalHub;
        public MyMedicalHistoryModel(IMedicalHistoryService medicalHistoryService, ISystemBalanceService systemBalanceService, ITransactionService transactionService,IHealthRecordService healthRecordService, IRatingsAndFeedbackService ratingsAndFeedbackService,IHubContext<SignalRServer> signalHub)
        {
            _medicalHistoryService = medicalHistoryService;
            _systemBalanceService = systemBalanceService;
            _transactionService = transactionService;
            _healthRecordService = healthRecordService;
            _ratingsAndFeedbackService = ratingsAndFeedbackService;
            _signalHub = signalHub;
        }
        public void OnGet(int pg = 1)
        {
            PreRender(pg);
        }

        public IActionResult  OnPostCancelMyMedicalHistory(int hid) {
            var item = _medicalHistoryService.GetById(hid);
            if (item != null) {
                item.Status = "Cancel";
                _medicalHistoryService.Update(item);
                var transaction = _transactionService.GetById((int)item.Appointment.TransactionId);
                //refund
                var transactionRefund = new Transaction {
                    BalanceId = 1,
                    UserId = item.Appointment.PatientId,
                    PaymentMethod = transaction.PaymentMethod,
                    Amount = transaction.Amount,
                    TransactionStatus = "success",
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    TransactionType = "RefundTransaction"
                };
                _transactionService.Create(transactionRefund);
                _systemBalanceService.update(1, -transaction.Amount);

            }
            int pg =1;
            PreRender(pg);
         
            return Page();

        }

      

        private void PreRender(int pg)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int pageSize = 5;

            // Lấy role của user
            var role = User.FindFirstValue(ClaimTypes.Role);

            // Khai báo biến trước khi dùng
            List<MedicalHistory> listByPagination = new List<MedicalHistory>();
            int total = 0;

            // Kiểm tra role và gọi service
            if (role == "Patient")
            {
                (listByPagination, total) = _medicalHistoryService.GetALlPagainationsByPatientId(pg, pageSize, userId);
            }
            else if (role == "MedicalExpert")
            {
                (listByPagination, total) = _medicalHistoryService.GetALlPagainationsByExpertId(pg, pageSize, userId);
            }

            // Khởi tạo phân trang
            Pager = new Pager(total, pg, pageSize);

            // Chuyển đổi danh sách thành ViewModel
            MedicalHistoryViewModels = listByPagination.Select(item => new MedicalHistoryViewModel
            {
                HistoryId = item.HistoryId,
                Description = item.Description,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                StartDate = item.Appointment.StartDate,
                EndDate = item.Appointment?.EndDate,
                AppointmentStatus = item.Appointment?.Status,
                patientId = item.Appointment.PatientId,
                expertId = item.Appointment.ExpertId,
                Pay = (bool)item.Payed,
                patientInfor = item.Appointment?.Patient?.PatientNavigation != null
                    ? $"{item.Appointment.Patient.PatientNavigation.FullName} {item.Appointment.Patient.PatientNavigation.PhoneNumber} {item.Appointment.Patient.PatientNavigation.Email}"
                    : "N/A",
                ExpertInfor = item.Appointment?.Expert?.Expert != null
                    ? $"{item.Appointment.Expert.Expert.FullName} {item.Appointment.Expert.Expert.PhoneNumber} {item.Appointment.Expert.Expert.Email}"
                    : "N/A"
            }).ToList();

        }
        public IActionResult OnPostMyCreate(int id) {
            var rating = _ratingsAndFeedbackService.findByHisId(id);
            if (rating==null) {
                if (User.FindFirstValue(ClaimTypes.Role) == "MedicalExpert")
                {
                    TempData["ErrorMessage"] = "Please waiting for updating rating and feedback from patient";
                    return RedirectToPage("/MedicalHistories/MyMedicalHistory");
                }
                return RedirectToPage("/RatingsAndFeedbacks/Create", new { intHis = id });
            }
            return RedirectToPage("/RatingsAndFeedbacks/Detail", new { id = rating.FeedbackId });
        }
    }

  
}
