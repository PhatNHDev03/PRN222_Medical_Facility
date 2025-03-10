using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicaiFacility.RazorPage.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;

        public EditModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public EditInputModel Input { get; set; } = new EditInputModel();

        public class EditInputModel
        {
            public int UserId { get; set; }

            [Required(ErrorMessage = "Full Name is required.")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "Invalid phone number format.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "User Type is required.")]
            public string UserType { get; set; }

            public string? BankAccount { get; set; }

            [Required(ErrorMessage = "Status is required.")]
            public bool? Status { get; set; }
        }

        public List<SelectListItem> UserTypeOptions { get; set; }
        public List<SelectListItem> StatusOptions { get; set; }

        public IActionResult OnGet(int id)
        {
            var user = _userService.FindById(id);
            if (user == null)
            {
                return NotFound();
            }

            Input = new EditInputModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                BankAccount = user.BankAccount,
                Status = user.Status
            };

            // Kh?i t?o danh sách UserType
            UserTypeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Patient", Text = "Patient" },
                new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
            };

            // Kh?i t?o danh sách Status
            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Active" },
                new SelectListItem { Value = "false", Text = "Inactive" }
            };

            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                UserTypeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                    new SelectListItem { Value = "MedicalExpert", Text = "Medical Expert" }
                };
                StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "true", Text = "Active" },
                    new SelectListItem { Value = "false", Text = "Inactive" }
                };
                return new JsonResult(new { success = false, message = "Invalid input data." });
            }

            var user = _userService.FindById(Input.UserId);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "User not found." });
            }

            try
            {
                user.FullName = Input.FullName;
                user.Email = Input.Email;
                user.PhoneNumber = Input.PhoneNumber;
                user.UserType = Input.UserType;
                user.BankAccount = Input.BankAccount;
                user.Status = Input.Status;
                user.UpdatedAt = DateTime.Now;

                _userService.UpdateUser(user);

                return new JsonResult(new { success = true, message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}