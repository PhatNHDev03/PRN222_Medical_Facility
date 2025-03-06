using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    public class IndexModel : PageModel
    {
        public List<MedicalFacility> MedicalFacilities { get; set; }
        public Pager Pager { get; set; }

        private readonly IMedicalFacilityService _medicalFacilityService;
        public IndexModel(IMedicalFacilityService medicalFacilityService)
        {
            _medicalFacilityService = medicalFacilityService;

        }
        public void OnGet(int pg = 1)
        {
            int pageSize = 5;
            var (medicalFacilitiesAll, totalItem) = _medicalFacilityService.FindAllWithPagination(pg, pageSize);
            Pager = new Pager(totalItem, pg, pageSize);
            MedicalFacilities = medicalFacilitiesAll;
        }
    }
}
