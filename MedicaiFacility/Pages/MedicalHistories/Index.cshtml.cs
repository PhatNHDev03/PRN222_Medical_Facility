using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.RazorPage.ViewModel;
using MedicaiFacility.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicaiFacility.RazorPage.Pages.MedicalHistories
{
    [BindProperties]
    public class IndexModel : PageModel
    {

        public List<MedicalHistoryViewModel> MedicalHistoryViewModels { get; set; } = new List<MedicalHistoryViewModel>();
        public Pager Pager { get; set; }
        private readonly IMedicalHistoryService _medicalHistoryService;
        public IndexModel(IMedicalHistoryService medicalHistoryService)
        {
            _medicalHistoryService = medicalHistoryService;
        }
        public void OnGet(int pg=1)
        {
            int pageSize = 5;
            var (listByPagination, total) = _medicalHistoryService.GetALlPagainations(pg,pageSize);
            Pager = new Pager(total, pg, pageSize);
            var checkPage =Pager.TotalPages;
            foreach (var item in listByPagination) {
                MedicalHistoryViewModels.Add(new MedicalHistoryViewModel {
                    HistoryId = item.HistoryId,
                    Description = item.Description,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    StartDate = item.Appointment.StartDate,
                    EndDate = item.Appointment.EndDate,
                    AppointmentStatus = item.Appointment.Status,
                    patientInfor= item.Appointment.Patient.PatientNavigation.FullName + " "+item.Appointment.Patient.PatientNavigation.PhoneNumber+" "+ item.Appointment.Patient.PatientNavigation.Email,
                    ExpertInfor = item.Appointment.Expert.Expert.FullName+ " " + item.Appointment.Expert.Expert.PhoneNumber+" "+ item.Appointment.Expert.Expert.Email
                });

                var check = MedicalHistoryViewModels.ToList().Count();
            }

        }
        public IActionResult OnPostDeleteById(int id) { 
            var item = _medicalHistoryService.GetById(id);
            if (item != null) {
                item.Status = "IsDeleted";
                _medicalHistoryService.Update(item);
            }
            return RedirectToPage("/MedicalHistories/Index");
        }
    }
}
