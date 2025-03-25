using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
		public EditModel(IMedicalHistoryService medicalHistoryService, IHealthRecordService healthRecordService)
		{
			_medicalHistoryService = medicalHistoryService;
			_healthRecordService = healthRecordService;
		}
		public void OnGet(int? id)
		{
			MedicalHistory = _medicalHistoryService.GetById((int)id);
			int pg = 1;
			PreRender(pg);
		}

		public IActionResult OnPost()
		{
			var exsisting  =  _medicalHistoryService.GetById(MedicalHistory.HistoryId);
			exsisting.Status = MedicalHistory.Status;
            exsisting.UpdatedAt = DateTime.Now;

			_medicalHistoryService.Update(exsisting);
			if (MedicalHistory.Status == "Processing")
			{
                TempData["SuccessMessage"] = "System is updated and you can update this medical examination";
            }
			if (MedicalHistory.Status == "Completed")
			{
				
			
					TempData["SuccessMessage"] = "Please update health record about this medical examination";

                    return RedirectToPage("/HealthRecords/Create", new { hisId = exsisting.HistoryId });
				
			}
			if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
			{
				return RedirectToPage("/MedicalHistories/MyMedicalHistory");
			}
			return RedirectToPage("/MedicalHistories/Index");
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
