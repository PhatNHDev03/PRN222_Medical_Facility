using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.Users
{
    public class PatientIndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;

        public PatientIndexModel(IUserService userService, IPatientService patientService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        public UserInfoModel UserInfo { get; set; }

        public class UserInfoModel
        {
            public int UserId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string BankAccount { get; set; }
            public string Image { get; set; }
            public string UserType { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Gender { get; set; }
        }

        public IActionResult OnGet()
        {
            // L?y UserId t? Claims
            int userId = int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int uid) ? uid : 0;
            if (userId == 0)
            {
                Console.WriteLine("User not authenticated, redirecting to login.");
                return RedirectToPage("/Users/Login");
            }

            Console.WriteLine($"Loading profile for UserId: {userId}");

            // L?y thông tin ng??i dùng t? UserService
            var user = _userService.FindById(userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return NotFound("User not found.");
            }

            Console.WriteLine($"User found: UserId={user.UserId}, UserType={user.UserType}, FullName={user.FullName}");

            // T?o UserInfoModel ?? truy?n d? li?u sang view
            UserInfo = new UserInfoModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BankAccount = user.BankAccount,
                Image = user.Image,
                UserType = user.UserType
            };

            // N?u ng??i dùng là Patient, l?y thêm thông tin DateOfBirth và Gender
            if (user.UserType == "Patient")
            {
                Console.WriteLine("User is a Patient, loading Patient info...");
                var patient = _patientService.GetPatientById(userId);
                if (patient != null)
                {
                    Console.WriteLine($"Patient found: DateOfBirth={patient.DateOfBirth}, Gender={patient.Gender}");
                    UserInfo.DateOfBirth = patient.DateOfBirth;
                    UserInfo.Gender = patient.Gender;
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
    }
}