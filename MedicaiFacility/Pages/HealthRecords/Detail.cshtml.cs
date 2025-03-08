using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    public class DetailModel : PageModel
    {
        public HealthRecord HealthRecord { get; set; }
        private readonly IHealthRecordService _healthRecordService;
        public DetailModel(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }
        public IActionResult OnGet(int? id)
        {
            HealthRecord = _healthRecordService.FindById((int)id);

            return Page();
        }
    }
}
