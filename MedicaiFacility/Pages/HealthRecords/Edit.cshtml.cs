using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public HealthRecord HealthRecord { get; set; }
        private readonly IHealthRecordService _healthRecordService;
        private readonly IDiseaseService _diseaseService;
        private readonly IHealthRecordDiseasesService _healthRecordDiseasesService;
        public List<SelectListItem> Diseases { get; set; }  // Danh sách tất cả bệnh
        public List<string> SelectedDiseaseIds { get; set; }  // Danh sách ID bệnh đã chọn

        public EditModel(IHealthRecordService healthRecordService, IDiseaseService diseaseService, IHealthRecordDiseasesService healthRecordDiseasesService)
        {
            _healthRecordService = healthRecordService;
            _diseaseService = diseaseService;
            _healthRecordDiseasesService = healthRecordDiseasesService;
        }

        public void OnGet(int? id)
        {
            HealthRecord = _healthRecordService.FindById((int)id);
            if (HealthRecord != null)
            {
                // Lấy danh sách tất cả bệnh
                Diseases = _diseaseService.GetAllDisease()
                    .Select(d => new SelectListItem
                    {
                        Value = d.DiseaseId.ToString(),
                        Text = d.DiseaseName + " - " + d.Department.DepartmentName
                    })
                    .ToList();

                // Lấy danh sách bệnh đã chọn
                SelectedDiseaseIds = HealthRecord.HealthRecordDiseases
                    .Where(d => d.DiseaseId.HasValue)
                    .Select(d => d.DiseaseId.Value.ToString())
                    .ToList();
            }
        }

        public async Task<IActionResult> OnPost(IFormFile file, List<string> SelectedDiseaseIds)
        {
            var existHealthRecord = _healthRecordService.FindById(HealthRecord.RecordId);
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
            else
            {
                HealthRecord.FilePath = existHealthRecord.FilePath;
            }

            existHealthRecord.FilePath = HealthRecord.FilePath;
            existHealthRecord.FileName = HealthRecord.FileName;
            existHealthRecord.TestResult = HealthRecord.TestResult;
            existHealthRecord.Diagnosis = HealthRecord.Diagnosis;
            existHealthRecord.Prescription = HealthRecord.Prescription;
            existHealthRecord.SharedLink = HealthRecord.SharedLink;
            existHealthRecord.UpdatedAt = DateTime.Now;

            var diseaseIdsToDelete = existHealthRecord.HealthRecordDiseases
            .Select(d => d.HealthRecordDiseaseId)
            .ToList(); // Chuyển sang List mới để tránh sửa đổi collection gốc

            foreach (var id in diseaseIdsToDelete)
            {
                _healthRecordDiseasesService.deleteById(id);
            }

            existHealthRecord.HealthRecordDiseases = SelectedDiseaseIds
                .Select(id => new HealthRecordDisease { DiseaseId = int.Parse(id) })
                .ToList();

            _healthRecordService.Udpate(existHealthRecord);
            return RedirectToPage("/HealthRecords/Detail", new { id = HealthRecord.RecordId });
        }
    }
}
