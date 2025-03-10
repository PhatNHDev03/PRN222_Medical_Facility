using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MedicaiFacility.RazorPage.Pages.Users
{
    public class SignUpModel : PageModel
    {
        private readonly IUserService _userService;

        public SignUpModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            [Required]
            public string UserType { get; set; } 
        }

        public List<SelectListItem> UserTypeOptions { get; set; }

        public void OnGet()
        {
            // T?o danh s�ch l?a ch?n cho UserType
            UserTypeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Patient", Text = "Patient" },
                new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // T?i l?i danh s�ch UserTypeOptions n?u validation th?t b?i
                UserTypeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                    new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
                };
                return Page();
            }

            try
            {
                var user = new User
                {
                    FullName = Input.FullName,
                    Email = Input.Email,
                    Password = Input.Password,
                    PhoneNumber = Input.PhoneNumber,
                    UserType = Input.UserType 
                };
                _userService.SignUp(user);
                return RedirectToPage("/Users/Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                UserTypeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                    new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
                };
                return Page();
            }
        }
    }
}