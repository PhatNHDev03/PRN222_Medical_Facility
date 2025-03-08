using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    public class DeleteModel : PageModel
    {
        private readonly IHealthRecordService _healthRecordService;
        public DeleteModel(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }
        public void OnGet(int id)
        {
         
        }

        public IActionResult OnPost(int? recordId) {
            var item = _healthRecordService.FindById((int)recordId);
            if (item != null) { 
                item.IsActive=false;
				_healthRecordService.Udpate(item);

			}
			return RedirectToPage("/HealthRecords/index");
        }
    }
}
