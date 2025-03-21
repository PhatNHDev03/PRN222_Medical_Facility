using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    [BindProperties]
    public class DetailModel : PageModel
    {
        public MedicalFacilityViewModel MedicalFacilityViewModels { get; set; }
        public List<MedicalExpert> Experts { get; set; } = new List<MedicalExpert>();

        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IMedicalExpertService _medicalExpertService;
        public DetailModel(IMedicalFacilityService medicalFacilityService, IMedicalExpertService medicalExpertService)
        {
            _medicalFacilityService = medicalFacilityService;
            _medicalExpertService = medicalExpertService;
        }
        public void OnGet(int id)
        {
            var x = _medicalFacilityService.FindById(id);
            MedicalFacilityViewModels =
        new MedicalFacilityViewModel
        {
            FacilityId = x.FacilityId,
            FacilityName = x.FacilityName,
            Address = x.Address,
            FacilityType = x.FacilityType,
            Verified = x.Verified,
            ContactInfo = x.ContactInfo,
            IsActive = x.IsActive,
            DepartmentName = x.FacilityDepartments.Select(fd => fd.Department.DepartmentName).ToList()
        };
            Experts = _medicalExpertService.getExpertsByFacilityId(id);
        }

    }
}
