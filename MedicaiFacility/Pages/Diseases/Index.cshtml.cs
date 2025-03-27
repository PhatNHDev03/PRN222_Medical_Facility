using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MedicaiFacility.Service;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.Diseases
{
    public class IndexModel : PageModel
    {
        public List<Disease> Diseases { get; set; } 
        public Pager Pager { get; set; } 

        private readonly IDiseaseService _diseaseService;
        public IndexModel(IDiseaseService diseaseService)
        {
            _diseaseService = diseaseService ?? throw new ArgumentNullException(nameof(diseaseService));
        }
        public IActionResult OnGet(int pg = 1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            int pageSize = 5;
            var (diseasesAll, totalItem) = _diseaseService.FindAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem, pg, pageSize);
            Diseases = diseasesAll;
            return Page();
        }
    }
}
