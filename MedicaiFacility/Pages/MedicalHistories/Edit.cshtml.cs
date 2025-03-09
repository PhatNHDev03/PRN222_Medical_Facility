using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalHistories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        public MedicalHistory MedicalHistory { get; set; }
        private readonly IMedicalHistoryService _medicalHistoryService;
        public EditModel(IMedicalHistoryService medicalHistoryService)
        {
            _medicalHistoryService = medicalHistoryService;
        }
        public void OnGet(int? id)
        {
            MedicalHistory = _medicalHistoryService.GetById((int)id);
        }

        public IActionResult OnPost() {
            var check = MedicalHistory;
            MedicalHistory.UpdatedAt = DateTime.Now;
            _medicalHistoryService.Update(MedicalHistory);
            return RedirectToPage("/MedicalHistories/Index");
        }
    }
}
