using MedicaiFacility.BusinessObject;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicaiFacility.RazorPage.Pages.Search
{
    [BindProperties]
    public class SearchFacilityModel : PageModel
    {
        public List<MedicalFacilityViewModel> MedicalFacilityViewModels { get; set; } = new List<MedicalFacilityViewModel>();
        public string? SearchTerm { get; set; }
        public string? Option { get; set; }

        public List<SelectListItem> DepartmentOptions { get; set; }
        public List<string>? SelectedDepartments { get; set; } = new();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IDepartmentService _departmentService;
        public SearchFacilityModel(IMedicalFacilityService medicalFacilityService, IDepartmentService departmentService)
        {
            _medicalFacilityService = medicalFacilityService;
            _departmentService = departmentService;
        }
        public void OnGet(string? searchTerm, string option = "Facility", int pg = 1, List<string>? selectedDepartments = null)
        {
            var departments = _departmentService.GetAllDepartment();
            DepartmentOptions = departments.Select(x => new SelectListItem
            {
                Value = x.DepartmentId + "",
                Text = x.DepartmentName
            }).ToList();
            SelectedDepartments = selectedDepartments ?? new List<string>();
            var listItem = _medicalFacilityService.GetAllMedicalFacility()
                .OrderByDescending(x => x.FacilityId).Where(x => x.IsActive == true && x.MedicalExperts.Any()).ToList();
            //filter by departMent

            var selectedDepartmentIds = SelectedDepartments
                .Where(id => int.TryParse(id, out _))
                .Select(int.Parse)
                .ToList();
            if (selectedDepartmentIds.Any())
            {
                listItem = listItem.Where(facility =>
                    facility.FacilityDepartments.Any(fd => selectedDepartmentIds.Contains(fd.DepartmentId ?? 0))
                ).ToList();
            }

            //filter by options
            var filterItem = new List<MedicalFacility>();
            SearchTerm = searchTerm;
            Option = option;
            if (option == "Facility")
            {
                filterItem = listItem.Where(x => string.IsNullOrEmpty(SearchTerm)
                || x.Address.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                || x.FacilityName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                || x.Service.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            if (option == "Hospital")
            {
                filterItem = listItem.Where(x => (string.IsNullOrEmpty(SearchTerm)
                    || x.Address.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || x.FacilityName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) && x.FacilityType == "Hospital"
                    ).ToList();
            }
            if (option == "Medical center")
            {
                filterItem = listItem.Where(x => (string.IsNullOrEmpty(SearchTerm)
                    || x.Address.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || x.FacilityName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) && x.FacilityType == "Medical center"
                    ).ToList();
            }
            if (option == "Clinic")
            {
                filterItem = listItem.Where(x => (string.IsNullOrEmpty(SearchTerm)
                    || x.Address.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || x.FacilityName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) && x.FacilityType == "Clinic"
                    ).ToList();
            }
            if (option == "Verified")
            {
                filterItem = listItem.Where(x => (string.IsNullOrEmpty(SearchTerm)
                 || x.Address.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                 || x.FacilityName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) && x.Verified == true
                 ).ToList();
            }

            //pagination
            int pageSize = 6;
            TotalPages = (int)Math.Ceiling((double)filterItem.Count / pageSize);
            CurrentPage = pg;
            var paginationList = filterItem.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            // chuyen thanh view model
            MedicalFacilityViewModels = paginationList
        .Select(x => new MedicalFacilityViewModel
        {
            FacilityId = x.FacilityId,
            FacilityName = x.FacilityName,
            Address = x.Address,
            FacilityType = x.FacilityType,
            Verified = x.Verified,
            ContactInfo = x.ContactInfo,
            Serivice = x.Service,
            IsActive = x.IsActive,
            DepartmentName = x.FacilityDepartments.Select(fd => fd.Department.DepartmentName).ToList()
        })
        .ToList();
        }
    }
}
