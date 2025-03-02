using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public List<HealthRecord> HealthRecords { get; set; }
        public Pager Pager { get; set; }
        private readonly IHealthRecordService _healthRecordService;
        public IndexModel(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }
            
        public void OnGet(int pg=1)
        {
            int pageSize = 5;
            var (HealthRecordWithPagination, totalItem) = _healthRecordService.findAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem,pg, pageSize);
         
            HealthRecords = HealthRecordWithPagination.ToList(); 
        }
    }
}
