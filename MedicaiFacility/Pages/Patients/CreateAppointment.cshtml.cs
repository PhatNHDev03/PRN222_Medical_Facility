using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    [BindProperties]
    public class CreateAppointmentModel : PageModel
    {
        public MedicalExpert MedicalExpert { get; set; }
        private IMedicalExpertService _medicalExpertService;
        private IHealthRecordService _healthRecordService;
        public CreateAppointmentModel(IMedicalExpertService medicalExpertService, IHealthRecordService healthRecordService)
        {
            _medicalExpertService = medicalExpertService;
            _healthRecordService = healthRecordService;
        }
        public void OnGet()
        {
            MedicalExpert = _medicalExpertService.getById(3);   
        }

        public IActionResult OnPostConfirmPayment(int expertId) {

            var check = _medicalExpertService.getById(expertId);

            // kiem tra thu Patient cua user co ton tai ko
            //tao transaction -->appoinment --> HEALTHRECORD


            return Page(); 
        }
    }
}
