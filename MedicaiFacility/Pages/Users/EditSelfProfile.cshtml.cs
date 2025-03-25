using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace MedicaiFacility.RazorPage.Pages.Users
{
    [BindProperties]
    public class EditSelfProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;

        public EditSelfProfileModel(IUserService userService, IPatientService patientService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public string OldPass { get; set; }
        public class InputModel
        {
            public int UserId { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [EmailAddress]
            public string NewEmail { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            public string? BankAccount { get; set; }

            [BindNever]
            public string Image { get; set; }

            public bool Status { get; set; }

            public string UserType { get; set; }

            [Required(ErrorMessage = "Date of Birth is required for Patients")]
            [Display(Name = "Date of Birth")]
            public DateTime? DateOfBirth { get; set; }

            [Required(ErrorMessage = "Gender is required for Patients")]
            public string Gender { get; set; }
        }

        public IActionResult OnGet(int id)
        {
       
            var user = _userService.FindById(id);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return NotFound("User not found.");
            }

            Console.WriteLine($"User found: UserId={user.UserId}, UserType={user.UserType}, FullName={user.FullName}");

            Input = new InputModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                NewEmail = user.Email,
                PhoneNumber = user.PhoneNumber,
                BankAccount = user.BankAccount,
                Image = user.Image,
                Status = user.Status ?? false,
                UserType = user.UserType
            };

            if (user.UserType == "Patient")
            {
                Console.WriteLine("User is a Patient, loading Patient info...");
                var patient = _patientService.GetPatientById(id);
                if (patient != null)
                {
                    Console.WriteLine($"Patient found: DateOfBirth={patient.DateOfBirth}, Gender={patient.Gender}");
                    Input.DateOfBirth = patient.DateOfBirth;
                    Input.Gender = patient.Gender;
                }
                else
                {
                    Console.WriteLine("No Patient record found for this user.");
                }
            }
            else
            {
                Console.WriteLine("User is not a Patient, skipping Patient fields.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            ModelState.Remove("Input.Image");

        

            var user = _userService.FindById(Input.UserId);
         

            if (user.Password != Password)
            {
                TempData["ErrorMessage"] = "Incorrect password.";

                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }
            if (user.Email!= Input.NewEmail && _userService.FindByEmail(Input.Email.Trim()) != null)
            {
                TempData["ErrorMessage"] = "Email is alreadly exist";

                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }
            if (user.PhoneNumber != Input.PhoneNumber && _userService.FindByPhoneNumber(Input.PhoneNumber.Trim()) != null)
            {
                TempData["ErrorMessage"] = "Phone Number is alreadly exist";

                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }
            user.FullName = Input.FullName;
            user.Email = Input.NewEmail;
            user.PhoneNumber = Input.PhoneNumber;
            user.BankAccount = Input.BankAccount;

            var profileImage = Request.Form.Files.GetFile("ProfileImage");

            if (profileImage != null && profileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                user.Image = $"/uploads/{fileName}";
            }
            else
            {
                Console.WriteLine("No profile image uploaded, keeping existing image.");
                var originalUser = _userService.FindById(Input.UserId);
                user.Image = originalUser?.Image;
            }

            try
            {
                // S? d?ng UpdateUserAndSessionAsync ?? c?p nh?t c? DB và session
                await _userService.UpdateUserAndSessionAsync(user, HttpContext);

                if (user.UserType == "Patient")
                {
                    var patient = _patientService.GetPatientById(Input.UserId);
                    if (patient != null)
                    {
                        patient.DateOfBirth = Input.DateOfBirth;
                        patient.Gender = Input.Gender;
                        _patientService.UpdatePatient(patient);
                    }
                    else
                    {
                        patient = new Patient
                        {
                            PatientId = Input.UserId,
                            DateOfBirth = Input.DateOfBirth,
                            Gender = Input.Gender
                        };
                        _patientService.CreatePatient(patient);
                    }
                }

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to update profile: {ex.Message}";
                return Page();
            }
        }

        public IActionResult OnPostChangePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                TempData["ErrorMessage"] = "All password fields are required.";
                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }

            if (newPassword != confirmNewPassword)
            {
                TempData["ErrorMessage"] = "New password and confirmation do not match.";
                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }

            var user = _userService.FindById(Input.UserId);
         

            if (user.Password != oldPassword)
            {
                TempData["ErrorMessage"] = "Old password is incorrect.";
                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }

            try
            {
                user.Password = newPassword;
                _userService.UpdateUser(user);
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to change password: {ex.Message}";
                return RedirectToPage("/Users/EditSelfProfile", new { id = Input.UserId });
            }
        }
    }
}