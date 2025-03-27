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
	[BindProperties]
	public class EditModel : PageModel
	{
		public List<MedicalHistoryViewModel> MedicalHistoryViewModels { get; set; } = new List<MedicalHistoryViewModel>();
		public Pager Pager { get; set; }
		public MedicalHistory MedicalHistory { get; set; }
		private readonly IMedicalHistoryService _medicalHistoryService;
		private readonly IHealthRecordService _healthRecordService;
        private readonly ISystemBalanceService _systemBalanceService;
        private readonly ITransactionService _transactionService;
        private readonly IHubContext<SignalRServer> _signalHub;
        public EditModel(IMedicalHistoryService medicalHistoryService, IHealthRecordService healthRecordService, 
			ISystemBalanceService systemBalanceService, ITransactionService transactionService, IHubContext<SignalRServer> signalHub)
		{
			_medicalHistoryService = medicalHistoryService;
			_healthRecordService = healthRecordService;
			_systemBalanceService = systemBalanceService;
			_transactionService = transactionService;
			_signalHub = signalHub;
		}
		public void OnGet(int? id)
		{
			MedicalHistory = _medicalHistoryService.GetById((int)id);
			int pg = 1;
			PreRender(pg);
		}

        // Trong EditModel.cs
        public async Task<IActionResult> OnPost()
        {
            var existing = _medicalHistoryService.GetById(MedicalHistory.HistoryId);
            existing.Status = MedicalHistory.Status;
            existing.UpdatedAt = DateTime.Now;

            try
            {
                if (MedicalHistory.Status == "Cancel")
                {
                    existing.Status = "Cancel";
                    _medicalHistoryService.Update(existing);

                    var transaction = _transactionService.GetById((int)existing.Appointment.TransactionId);
                    var transactionRefund = new Transaction
                    {
						BalanceId = 1,
                        UserId = existing.Appointment.PatientId,
                        PaymentMethod = transaction.PaymentMethod,
                        Amount = transaction.Amount,
                        TransactionStatus = "success",
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        TransactionType = "RefundTransaction"
                    };

                    _transactionService.Create(transactionRefund);
                    _systemBalanceService.update(1, -transaction.Amount);
                    _medicalHistoryService.Update(existing);

                    // Gửi signal đến tất cả client
                    await _signalHub.Clients.All.SendAsync("ReceiveMedicalHistoryUpdate");
                    return User.FindFirstValue(ClaimTypes.Role) != "Admin"
                    ? RedirectToPage("/MedicalHistories/MyMedicalHistory")
                    : RedirectToPage("/MedicalHistories/Index");
                }

                // Các case khác giữ nguyên
                if (MedicalHistory.Status == "Processing")
                {
                    TempData["SuccessMessage"] = "System is updated and you can update this medical examination";
                    _medicalHistoryService.Update(existing);
                    await _signalHub.Clients.All.SendAsync("ReceiveMedicalHistoryUpdate");
                }
                if (MedicalHistory.Status == "Completed")
                {
                    _medicalHistoryService.Update(existing);
                    await _signalHub.Clients.All.SendAsync("ReceiveMedicalHistoryUpdate");
                    TempData["SuccessMessage"] = "Please update health record about this medical examination";
                    return User.FindFirstValue(ClaimTypes.Role) != "Admin"
                  ? RedirectToPage("/HealthRecords/Create", new { hisId = existing.HistoryId })
                  : RedirectToPage("/MedicalHistories/Index");
                  
                }

                await _signalHub.Clients.All.SendAsync("ReceiveMedicalHistoryUpdate");
                return User.FindFirstValue(ClaimTypes.Role) != "Admin"
                    ? RedirectToPage("/MedicalHistories/MyMedicalHistory")
                    : RedirectToPage("/MedicalHistories/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the medical history";
                Console.Error.WriteLine(ex);
                return Page();
            }
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

				patientInfor = item.Appointment?.Patient?.PatientNavigation != null
					? $"{item.Appointment.Patient.PatientNavigation.FullName} {item.Appointment.Patient.PatientNavigation.PhoneNumber} {item.Appointment.Patient.PatientNavigation.Email}"
					: "N/A",
				ExpertInfor = item.Appointment?.Expert?.Expert != null
					? $"{item.Appointment.Expert.Expert.FullName} {item.Appointment.Expert.Expert.PhoneNumber} {item.Appointment.Expert.Expert.Email}"
					: "N/A"
			}).ToList();

		}
	}
}
