using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Diseases
{
    public class IndexModel : PageModel
    {
        public List<Disease> Diseases { get; set; } = new List<Disease>();
        public Pager Pager { get; set; } = new Pager(0, 1, 5);

        private readonly IDiseaseService _diseaseService;
        public IndexModel(IDiseaseService diseaseService)
        {
            _diseaseService = diseaseService ?? throw new ArgumentNullException(nameof(diseaseService));
        }
        public IActionResult OnGet(int pg = 1)
        {
            int pageSize = 5;
            try
            {
                var (diseasesAll, totalItem) = _diseaseService.FindAllWithPagination(pg, pageSize);
                if (diseasesAll == null)
                {
                    diseasesAll = new List<Disease>(); // Default to empty list if null
                    totalItem = 0; // Reset totalItem if diseasesAll is null
                }
                Pager = new Pager(totalItem, pg, pageSize);
                Diseases = diseasesAll;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching diseases: {ex.Message}");
                Diseases = new List<Disease>(); // Fallback to empty list on error
                Pager = new Pager(0, pg, pageSize); // Reset pager on error
            }
            return Page();
        }
    }
}
