using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.HealthRecords
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public List<HealthRecord> HealthRecord { get; set; }
		public List<HealthRecordViewModel> HealthRecordViewModel { get; set; } = new List<HealthRecordViewModel>();
		public User Expert { get; set; }
        public User Patient { get; set; }
        public Pager Pager { get; set; }
        private readonly IHealthRecordService _healthRecordService;
        private readonly IUserService _userService;
        public IndexModel(IHealthRecordService healthRecordService,IUserService userService)
        {
            _healthRecordService = healthRecordService;
            _userService = userService;
        }
            
        public IActionResult OnGet(int pg=1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            int pageSize = 5;
            var (HealthRecordWithPagination, totalItem) = _healthRecordService.findAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem,pg, pageSize);
			
			HealthRecord = HealthRecordWithPagination.ToList();
            foreach (var item in HealthRecord) {
                HealthRecordViewModel.Add(
                    new HealthRecordViewModel
                    {
                        RecordId = item.RecordId,
						
                        FileName = item.FileName,
                        FilePath = item.FilePath,
                        TestResult = item.TestResult,
                        Diagnosis = item.Diagnosis,
                        Prescription = item.Prescription,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        SharedLink = item.SharedLink,
                        IsActive = item.IsActive
					}
                    );
			}
            return Page();
        }
    }
}
