using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    [BindProperties]
    public class DetailModel : PageModel
    {
        public MedicalFacilityViewModel MedicalFacilityViewModels { get; set; }
        public int MedicalFacilityId { get; set; }
        public List<MedicalExpert> Experts { get; set; } = new List<MedicalExpert>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IMedicalExpertService _medicalExpertService;
        public DetailModel(IMedicalFacilityService medicalFacilityService, IMedicalExpertService medicalExpertService)
        {
            _medicalFacilityService = medicalFacilityService;
            _medicalExpertService = medicalExpertService;
        }
        public void OnGet(int id, int pg=1)
        {
            MedicalFacilityId = id;
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
            // display exxpertr co sheldue 
            var expertAll = _medicalExpertService.getExpertsByFacilityId(id).Where(x=>x.MedicalExpertSchedules.Any()).ToList();   
            int pageSize = 3;
            TotalPages = (int)Math.Ceiling((double)expertAll.Count / pageSize);
            CurrentPage = pg;
            var paginationList = expertAll.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            Experts = paginationList;
        }

    }
}
