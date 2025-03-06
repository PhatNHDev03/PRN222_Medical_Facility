using Azure;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Diseases
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IDiseaseService _diseaseService;
        private readonly IDepartmentService _departmentService;

        public Disease Disease { get; set; }
        public List<Department> Departments { get; set; }

        public EditModel(IDiseaseService diseaseService, IDepartmentService departmentService)
        {
            _diseaseService = diseaseService;
            _departmentService = departmentService;
        }

        public IActionResult OnGet(int id)
        {
            Departments = _departmentService.GetAllDepartment();
            Disease = _diseaseService.FindById(id);
            if (Disease == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _diseaseService.UpdateDisease(Disease);
            return RedirectToPage("Index");
        
        }
    }
}
