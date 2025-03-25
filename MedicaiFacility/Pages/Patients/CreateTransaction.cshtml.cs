using MedicaiFacility.BusinessObject;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MedicaiFacility.RazorPage.Pages.Patients
{
    [BindProperties]
    public class CreateTransactionModel : PageModel
    {

        public MedicalExpert MedicalExpert { get; set; }
        private IMedicalExpertService _medicalExpertService;
        private IHealthRecordService _healthRecordService;
        private IPatientService _patientService;
        private IUserService _userService;
        private ITransactionService _transactionService;
        private ISystemBalanceService _systemBalanceService;
        private IAppointmentService _appointmentService;
        public CreateTransactionModel(IMedicalExpertService medicalExpertService, IHealthRecordService healthRecordService, IPatientService patientService,
            IUserService userService, ITransactionService transactionService, ISystemBalanceService systemBalanceService, IAppointmentService appointmentService)
        {
            _medicalExpertService = medicalExpertService;
            _healthRecordService = healthRecordService;
            _patientService = patientService;
            _userService = userService;
            _transactionService = transactionService;
            _systemBalanceService = systemBalanceService;
            _appointmentService = appointmentService;
        }
        public void OnGet(int expertId)
        {
            MedicalExpert = _medicalExpertService.getById(expertId);
        }

        public IActionResult OnPostConfirmTransaction(int expertId, string PaymentMethodSelected, decimal AmountAccepted)
        {

            MedicalExpert = _medicalExpertService.getById(expertId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userService.FindById(int.Parse(userId));
            var userParse = int.Parse(userId);
            if (user.BankAccount == null)
            {
                ViewData["ErrorMessage"] = "Please update your bank account to book expert";
                var check = ViewData["ErrorMessage"];
                return Page();
            }
            var patientProfile = _patientService.getById(int.Parse(userId));
            // check thu co profile patient 
            if (patientProfile == null)
            {
                return RedirectToPage("/Patients/NotFoundProfile");
            }
            var system = _systemBalanceService.GetBalance(1);
            var transaction = new Transaction
            {
                Balance = system,
                UserId = int.Parse(userId),
                PaymentMethod = PaymentMethodSelected,
                Amount = AmountAccepted,
                TransactionStatus = "Success",
                CreatedAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                TransactionType = "AppointmentTransaction"
            };
            _transactionService.Create(transaction);
            // luu amount vao system balance ()
            _systemBalanceService.update(1, AmountAccepted);

           


            return RedirectToPage("/Patients/CreateAppointment", new { patientId = userParse , transactionId = transaction .TransactionId, expertId = MedicalExpert.ExpertId });
        }
    }
}
