using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Search
{
    public class SearchDoctorsModel : PageModel
    {
        private readonly IMedicalExpertService _medicalExpertService;

        public SearchDoctorsModel(IMedicalExpertService medicalExpertService)
        {
            _medicalExpertService = medicalExpertService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<DoctorViewModel> Doctors { get; set; }

        public void OnGet()
        {
            var medicalExperts = _medicalExpertService.SearchDoctors(SearchTerm);

            Doctors = medicalExperts.Select(me => new DoctorViewModel
            {
                ExpertId = me.ExpertId,
                FullName = me.Expert?.FullName ?? "Unknown",
                Image = me.Expert?.Image ?? "https://cdn.medpro.vn/prod-partner/3ee52d40-60a4-492b-af06-f759cce2d5d2-doctormale.jpg", 
                Specialization = me.Specialization ?? "N/A",
                ExperienceYears = me.ExperienceYears,
                DepartmentName = me.Department ?? "N/A",
                FacilityName = me.Facility?.FacilityName ?? "N/A",
                Address = me.Facility?.Address ?? "N/A",
                PriceBooking = me.PriceBooking ?? 0,
            }).ToList();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new { SearchTerm });
        }
    }
}
