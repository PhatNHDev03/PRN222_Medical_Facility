using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace MedicaiFacility.RazorPage.Pages.Users
{
    [Authorize]
    [BindProperties]
    public class EditSelfProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment; // ?? truy c?p th? m?c wwwroot

        public EditSelfProfileModel(IUserService userService, IWebHostEnvironment environment)
        {
            _userService = userService;
            _environment = environment;
            Input = new EditSelfProfileInputModel();
        }

        public EditSelfProfileInputModel Input { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        [BindProperty]
        public string? OldPassword { get; set; }

        [BindProperty]
        public string? NewPassword { get; set; }

        [BindProperty]
        public string? ConfirmNewPassword { get; set; }

        [BindProperty]
        public IFormFile? ProfileImage { get; set; } // Thu?c tính ?? nh?n file ?nh

        public class EditSelfProfileInputModel
        {
            public int UserId { get; set; }

            [Required(ErrorMessage = "Full Name is required.")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Current Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "New Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string NewEmail { get; set; }

            [Phone(ErrorMessage = "Invalid phone number format.")]
            public string? PhoneNumber { get; set; }

            public string? BankAccount { get; set; }

            public string? Image { get; set; } // Thêm thu?c tính Image

            public bool Status { get; set; }

            public string UserType { get; set; }
        }

        public IActionResult OnGet()
        {
            string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "User not authenticated!";
                return RedirectToPage("/Index");
            }

            var user = _userService.FindByEmail(userEmail);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToPage("/Index");
            }

            Input = new EditSelfProfileInputModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                NewEmail = user.Email,
                PhoneNumber = user.PhoneNumber,
                BankAccount = user.BankAccount,
                Image = user.Image, // L?y ???ng d?n ?nh
                Status = user.Status ?? false,
                UserType = user.UserType
            };

            return Page();
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            ModelState.Remove("OldPassword");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmNewPassword");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = _userService.FindById(Input.UserId);
            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToPage("EditSelfProfile");
            }

            if (!string.IsNullOrEmpty(Password) && !_userService.ValidatePassword(existingUser.Email, Password))
            {
                TempData["ErrorMessage"] = "Sai m?t kh?u!";
                return RedirectToPage("EditSelfProfile");
            }

            bool isEmailChanged = !existingUser.Email.Equals(Input.NewEmail);

            if (isEmailChanged)
            {
                var checkEmail = _userService.IsExistEmail(Input.NewEmail.Trim());
                if (checkEmail != null && checkEmail.UserId != Input.UserId)
                {
                    TempData["ErrorMessage"] = "Email is already in use!";
                    return RedirectToPage("EditSelfProfile");
                }
            }

            // X? lý upload ?nh
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // ???ng d?n l?u ?nh trong wwwroot/images
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder); // T?o th? m?c n?u ch?a t?n t?i
                }

                // T?o tên file duy nh?t ?? tránh trùng l?p
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // L?u file vào th? m?c
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                // C?p nh?t ???ng d?n ?nh vào c?t Image
                existingUser.Image = $"/images/{uniqueFileName}";
            }

            // C?p nh?t thông tin ng??i dùng
            existingUser.FullName = Input.FullName;
            if (isEmailChanged)
            {
                existingUser.Email = Input.NewEmail;
            }
            existingUser.PhoneNumber = Input.PhoneNumber;
            existingUser.BankAccount = Input.BankAccount;
            existingUser.Status = Input.Status;
            existingUser.UpdatedAt = DateTime.Now;

            _userService.UpdateUser(existingUser);

            // C?p nh?t session v?i thông tin m?i
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingUser.FullName),
                new Claim(ClaimTypes.Email, existingUser.Email),
                new Claim(ClaimTypes.Role, existingUser.UserType)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("EditSelfProfile");
        }

        public IActionResult OnPostChangePassword()
        {
            ModelState.Remove("Input.FullName");
            ModelState.Remove("Input.Email");
            ModelState.Remove("Input.NewEmail");
            ModelState.Remove("Input.PhoneNumber");
            ModelState.Remove("Input.BankAccount");
            ModelState.Remove("Input.Status");
            ModelState.Remove("Input.UserType");
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                return RedirectToPage("EditSelfProfile");
            }

            var existingUser = _userService.FindById(Input.UserId);
            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToPage("EditSelfProfile");
            }

            if (!string.IsNullOrEmpty(OldPassword) && !_userService.ValidatePassword(existingUser.Email, OldPassword))
            {
                TempData["ErrorMessage"] = "Sai m?t kh?u c?!";
                return RedirectToPage("EditSelfProfile");
            }

            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword) || NewPassword != ConfirmNewPassword)
            {
                TempData["ErrorMessage"] = "New password and confirmation do not match or are empty!";
                return RedirectToPage("EditSelfProfile");
            }

            _userService.UpdateUser(existingUser);
            _userService.ChangePassword(existingUser.Email, NewPassword);

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToPage("EditSelfProfile");
        }
    }
}