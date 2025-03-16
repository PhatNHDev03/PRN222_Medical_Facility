using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    public class DetailModel : PageModel
    {
        public HealthRecord HealthRecord { get; set; }
        private readonly IHealthRecordService _healthRecordService;
        private readonly IDiseaseService _diseaseService;
		public List<SelectListItem> SelectedDiseases { get; set; }
		public DetailModel(IHealthRecordService healthRecordService,IDiseaseService diseaseService)
        {
            _healthRecordService = healthRecordService;
            _diseaseService = diseaseService;
        }
		public void OnGet(int? id)
		{
		

			HealthRecord = _healthRecordService.FindById((int)id);

			if (HealthRecord != null)
			{
				// Lấy danh sách tất cả bệnh
				var allDiseases = _diseaseService.GetAllDisease()
					.ToDictionary(d => d.DiseaseId, d => d.DiseaseName + " - " + d.Department.DepartmentName);

				// Lọc danh sách bệnh đã chọn từ HealthRecord
				SelectedDiseases = HealthRecord.HealthRecordDiseases
					.Where(d => d.DiseaseId.HasValue && allDiseases.ContainsKey(d.DiseaseId.Value)) // Kiểm tra DiseaseId hợp lệ
					.Select(d => new SelectListItem { Value = d.DiseaseId.ToString(), Text = allDiseases[d.DiseaseId.Value] })
					.ToList();
			}
		}



		public IActionResult OnGetHisId(int? id) { 
            
            HealthRecord = _healthRecordService.findByMedicalHistoryId((int)id);
            return Page();
        }
    }
}
