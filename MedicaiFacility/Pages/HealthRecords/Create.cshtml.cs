using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public string SelectedDiseaseIds { get; set; } = "";

        public List<SelectListItem> Diseases { get; set; }

        private readonly IHealthRecordService _healthRecordService;
        private readonly IDiseaseService _diseaseService;

        public CreateModel(IHealthRecordService healthRecordService, IDiseaseService diseaseService)
        {
            _healthRecordService = healthRecordService;
            _diseaseService = diseaseService;
        }

        public HealthRecord HealthRecord { get; set; }

        public IActionResult OnGet()
        {
            Diseases = _diseaseService.GetAllDisease()
                .Where(d => d.IsActive==true)
                .Select(d => new SelectListItem { Value = d.DiseaseId.ToString(), Text = d.DiseaseName+"- "+d.Department.DepartmentName})
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "imgPatient");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                HealthRecord.FilePath = "/imgPatient/" + file.FileName;
            }

            // Chuyển danh sách checkbox thành list ID
            List<int> diseaseIds = new List<int>();
            if (!string.IsNullOrEmpty(SelectedDiseaseIds))
            {
                diseaseIds = SelectedDiseaseIds.Split(',')
                                              .Where(id => int.TryParse(id, out _))
                                              .Select(int.Parse)
                                              .ToList();
            }

            
            // Gán danh sách bệnh vào HealthRecord
            HealthRecord.HealthRecordDiseases = diseaseIds
                .Select(diseaseId => new HealthRecordDisease { RecordId = HealthRecord.RecordId, DiseaseId = diseaseId })
                .ToList();

            HealthRecord.CreatedAt = DateTime.Now;
            HealthRecord.UpdatedAt = DateTime.Now;
            HealthRecord.IsActive = true;

            _healthRecordService.Save(HealthRecord);

            TempData["SuccessMessage"] = "Health record saved successfully";
            return Page();
        }
    }
}
