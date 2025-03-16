using MedicaiFacility.BusinessObject;
using System.Collections.Generic;

namespace MedicaiFacility.DataAccess.IRepostory
{
    public interface IMedicalExpertRepository
    {
        MedicalExpert getById(int id);
        void Add(MedicalExpert medicalExpert);
        IEnumerable<MedicalExpert> GetAllMedicalExperts();
        void Update(MedicalExpert medicalExpert);
        void Delete(int id);
        void AddMedicalExpertSchedule(MedicalExpertSchedule schedule);
        void DeleteSchedulesByExpertId(int expertId);
    }
}