using MedicaiFacility.BusinessObject;

using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages
{
    [BindProperties]
    public class HealthRecordModel : PageModel
    {
        public List<HealthRecord> healthRecords { get; set; }
        private readonly IHealthRecordService _healthRecordService;
        public HealthRecordModel(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        public void OnGet()
        {
            healthRecords = _healthRecordService.GetAll();
        }
    }
}
