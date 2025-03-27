using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly IUserRepository _userRepository;

        public IndexModel(IPatientService patientService, IUserRepository userRepository)
        {
            _patientService = patientService;
            _userRepository = userRepository ;
        }

        public IList<Patient> Patients { get; set; }
        public IList<User> UserList { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
  

        public IActionResult OnGet(int pg = 1)
        {
            if (User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return RedirectToPage("/Index");
            }
            var allPatients = _patientService.GetAllPatients().ToList();

            int totalPatients = allPatients.Count;
            int pageSize = 5;
            TotalPages = (int)Math.Ceiling((double)allPatients.Count / pageSize);
            CurrentPage = pg;
            Patients = allPatients.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            return Page();
        }




    }
}