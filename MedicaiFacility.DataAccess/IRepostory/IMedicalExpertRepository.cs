using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess.IRepostory
{
    public interface IMedicalExpertRepository
    {
        Task<MedicalExpert> getByIdAsync(int id); // Updated to async
        List<MedicalExpert> GetAllMedicalExpert();
        void UpdateMedicalExpert(MedicalExpert medicalExpert);
        void DeleteMedicalExpert(int id);
        MedicalExpert getById(int id);
        (List<MedicalExpert>, int totalItem) FindAllWithPagination(int pg, int pageSize);
        List<MedicalExpert> SearchDoctors(string searchTerm);
        List<string> GetScheduleByExpertId(int expertId);
        List<RatingsAndFeedback> GetFeedbacksByExpertId(int expertId);
        Task UpdateScheduleAsync(int expertId, List<string> selectedDays);
        List<MedicalExpert> getExpertsByFacilityId(int facilityId);

    }
}
