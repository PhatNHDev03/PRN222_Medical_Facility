using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace MedicaiFacility.RazorPage.Pages.MedicalFacilites
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IMedicalFacilityService _medicalFacilityService;

        public MedicalFacility MedicalFacility { get; set; }

        public CreateModel(IMedicalFacilityService medicalFacilityService)
        {
            _medicalFacilityService = medicalFacilityService;
        }
        public IActionResult OnGet()
        {
            MedicalFacility = new MedicalFacility
            {
                Verified = true, // Default value 
                IsActive = true    // Default value 
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            var validFacilityTypes = new[] { "Public Hospital", "Private Hospital", "Clinic" };
            if (string.IsNullOrEmpty(MedicalFacility.FacilityType) || !validFacilityTypes.Contains(MedicalFacility.FacilityType))
            {
                ModelState.AddModelError("MedicalFacility.FacilityType", "Invalid Facility Type. Must be one of: " + string.Join(", ", validFacilityTypes));
                return new JsonResult(new { success = false, message = "Invalid data! Invalid Facility Type." });
            }


            Console.WriteLine($"Received MedicalFacility: Name={MedicalFacility.FacilityName}, Address={MedicalFacility.Address}, " +
                $"FacilityType={MedicalFacility.FacilityType}, Verified={MedicalFacility.Verified}, " +
                $"ContactInfo={MedicalFacility.ContactInfo}, IsActive={MedicalFacility.IsActive}");
            try
            {
                _medicalFacilityService.AddMedicalFacility(MedicalFacility);
                return new JsonResult(new { success = true, message = "MedicalFacility created successfully!", redirectUrl = "/MedicalFacilities/Index" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.ToString());
                return new JsonResult(new { success = false, message = $"Error creating MedicalFacility: {ex.Message}" });
            }
        }

    }
}
