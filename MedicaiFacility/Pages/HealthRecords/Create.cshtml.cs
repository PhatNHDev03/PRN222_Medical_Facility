using MedicaiFacility.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    [BindProperties]
    public class CreateModel : PageModel
    {

        public HealthRecord HealthRecord { get; set; }
        public IActionResult OnGet()
        {
            HealthRecord.PatientId = 2;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
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

            return Page();
        }


    }
}
