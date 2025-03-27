using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;
using MedicaiFacility.Service.IService;

namespace MedicaiFacility.RazorPage.Pages.MedicalExperts
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMedicalHistoryService _medicalHistoryService;
        [BindProperty]
        public User MedicalExpert { get; set; }
        public DeleteModel(IUserService userService, ITransactionService transactionService, IAppointmentService appointmentService, IMedicalHistoryService medicalHistoryService)
        {
            _userService = userService;
            _transactionService = transactionService;
            _appointmentService = appointmentService;
            _medicalHistoryService = medicalHistoryService;
        }

        public IActionResult OnGet(int id)
        {
            MedicalExpert = _userService.FindById(id);
            if (MedicalExpert == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (MedicalExpert == null)
            {
                return NotFound();
            }

            try
            {
                var existing = _userService.FindById(MedicalExpert.UserId);

                existing.Status = false;

                var medicalHistory = _medicalHistoryService.GetAll().Where(x => x.Status == "Pending"|| x.Status == "Processing").ToList();
                if (medicalHistory.Count()>0) {
                    TempData["SuccessMessage"] = "MedicalExpert failed to delete!, There are still have pending medical history";
                    return RedirectToPage("/MedicalExperts/Index");
                }
               
                var checkPendingAppoinment = _appointmentService.GetAll().Where(x => x.ExpertId == existing.UserId && x.Status == "Pending").ToList();
                if (checkPendingAppoinment.Count()>0) {
                    List<Transaction> transactionExsiting = new List<Transaction>();
                    foreach (var item in checkPendingAppoinment) {                     
                        transactionExsiting.Add(_transactionService.GetById((int)item.TransactionId));
                    }
                    List<Transaction> refundTransactions = new List<Transaction>();
                    foreach (var transaction in transactionExsiting) {
                        _transactionService.Create(new Transaction
                        {
                            BalanceId = 1,
                            UserId = transaction.UserId,
                            PaymentMethod = transaction.PaymentMethod,
                            Amount = transaction.Amount,
                            TransactionStatus = "Success",
                            CreatedAt = DateTime.Now,
                            UpdateAt = DateTime.Now,
                            TransactionType = "RefundTransaction"
                        }); 
                    }
                }
                _userService.UpdateUser(existing);
                TempData["SuccessMessage"] = "MedicalExpert deleted successfully!";
                return RedirectToPage("/MedicalExperts/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete MedicalExpert: {ex.Message}";
                return Page();
            }
        }
    }
}