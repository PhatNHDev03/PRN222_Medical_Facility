using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MedicaiFacility.RazorPage.Pages.Users
{
    [BindProperties]
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
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string RePassword { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            [Required]
            public string UserType { get; set; }

            public string BankAccount { get; set; }
        }

        public List<SelectListItem> UserTypeOptions { get; set; }

        public void OnGet()
        {
            Pre();
        }
        private void Pre() {
            // T?o danh sách l?a ch?n cho UserType
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
                UserTypeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                    new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
                };
                return Page();
            }
            if (_userService.FindByEmail(Input.Email.Trim())!=null) {
                TempData["Error"] = "Email is alreadly exist";
                Pre();
                return Page();
            }
            if (_userService.FindByPhoneNumber(Input.PhoneNumber.Trim()) != null)
            {
                TempData["Error"] = "Phone Number is alreadly exist";
                Pre();
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
                    UserType = Input.UserType,
                    CreatedAt= DateTime.Now,
                    UpdatedAt= DateTime.Now,
                    IsApprove = (Input.UserType== "Patient")?true:false,
                    BankAccount = Input.BankAccount,
                    Status = true
                };
                _userService.Add(user);
                if (user.UserType== "Patient") {
                    return RedirectToPage("/Patients/Create", new {id = user.UserId });
                }
                return RedirectToPage("/MedicalExperts/Create",new { id= user.UserId});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                UserTypeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                    new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
                };
                Pre();
                return Page();
            }
        }
    }
}