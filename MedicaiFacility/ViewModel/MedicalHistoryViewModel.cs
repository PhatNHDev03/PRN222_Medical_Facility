using MedicaiFacility.BusinessObject;

namespace MedicaiFacility.RazorPage.ViewModel
{
    public class MedicalHistoryViewModel
    {
        public int? HistoryId { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        //appoinment

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? AppointmentId { get; set; }
        public string? AppointmentStatus { get; set; }
        //user
        public string? patientInfor { get; set; }

        public string? ExpertInfor { get; set; }

    }
}
