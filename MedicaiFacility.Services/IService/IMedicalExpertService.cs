using MedicaiFacility.BusinessObject;
using System.Collections.Generic;

namespace MedicaiFacility.Service.IService
{
    public interface IMedicalExpertService
    {
        MedicalExpert getById(int id);
        void CreateMedicalExpert(MedicalExpert medicalExpert);
        IEnumerable<MedicalExpert> GetAllMedicalExperts();
        void UpdateMedicalExpert(MedicalExpert medicalExpert);
        void DeleteMedicalExpert(int id);
        void AddMedicalExpertSchedule(MedicalExpertSchedule schedule);
        void DeleteSchedulesByExpertId(int expertId);
            List<MedicalExpert> SearchDoctors(string searchTerm);
        List<string> GetScheduleByExpertId(int expertId);
        List<RatingsAndFeedback> GetFeedbacksByExpertId(int expertId);
        List<MedicalExpert> getExpertsByFacilityId(int facilityId);
    }
}