using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalHistories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        public MedicalHistory MedicalHistory { get; set; }
        private readonly IMedicalHistoryService _medicalHistoryService;
        public CreateModel(IMedicalHistoryService medicalHistoryService)
        {
                _medicalHistoryService = medicalHistoryService;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var check = MedicalHistory;
            MedicalHistory.CreatedAt = DateTime.Now;
            MedicalHistory.UpdatedAt = DateTime.Now;
            _medicalHistoryService.Create(MedicalHistory);
            return RedirectToPage("/MedicalHistories/Index");
        }
    }
}
