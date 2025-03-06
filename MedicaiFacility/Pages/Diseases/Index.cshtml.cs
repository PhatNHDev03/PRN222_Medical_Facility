using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Diseases
{
    public class IndexModel : PageModel
    {
        public List<Disease> Diseases { get; set; }
        public Pager Pager { get; set; }

        private readonly IDiseaseService _diseaseService;
        public IndexModel(IDiseaseService diseaseService)
        {
            _diseaseService = diseaseService;

        }
        public void OnGet(int pg = 1)
        {
            int pageSize = 5;
            var (diseasesAll, totalItem) = _diseaseService.FindAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem, pg, pageSize);
            Diseases = diseasesAll;
        }
    }
}
