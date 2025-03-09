using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public HealthRecord HealthRecord { get; set; }
        private readonly IHealthRecordService _healthRecordService;
        public EditModel(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }
        public IActionResult OnGet(int? id)
        {
            HealthRecord = _healthRecordService.FindById((int)id);
            
            return Page();
        }
        public async Task<IActionResult> OnPost(IFormFile file) {
            if (file != null && file.Length > 0)
            {
                // Định nghĩa thư mục lưu file
                var uploadsFolder = Path.Combine("wwwroot", "imgPatient");
                Directory.CreateDirectory(uploadsFolder); // Đảm bảo thư mục tồn tại

                // Tạo đường dẫn đầy đủ
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                // Lưu file vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Lưu đường dẫn để hiển thị
                HealthRecord.FilePath = "/imgPatient/" + file.FileName;
            }

            var check = HealthRecord;
            _healthRecordService.Udpate(HealthRecord);
			return RedirectToPage("/HealthRecords/Detail", new { id = HealthRecord.RecordId });

		}
	}
}
