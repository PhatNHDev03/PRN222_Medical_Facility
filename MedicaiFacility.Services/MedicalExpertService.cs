using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service
{
    internal class MedicalExpertService : IMedicalExpertService
    {
        private readonly IMedicalExpertRepository _medicalExpertRepository;
        public MedicalExpertService(IMedicalExpertRepository medicalExpertRepository)
        {
            _medicalExpertRepository = medicalExpertRepository;
        }
        public async Task<MedicalExpert> GetByIdAsync(int id)
        {
            return await _medicalExpertRepository.getByIdAsync(id);
        }

        public MedicalExpert getById(int id)
        {
           return _medicalExpertRepository.getById(id);
        }
        public (List<MedicalExpert>, int totalItem) FindAllWithPagination(int pg, int pageSize)
        {
            return _medicalExpertRepository.FindAllWithPagination(pg, pageSize);
        }
        public List<MedicalExpert> GetAllMedicalExpert()
        {
            return _medicalExpertRepository.GetAllMedicalExpert();
        }

        public void UpdateMedicalExpert(MedicalExpert medicalExpert)
        {
            _medicalExpertRepository.UpdateMedicalExpert(medicalExpert);
        }

        public void DeleteMedicalExpert(int id)
        {
            _medicalExpertRepository.DeleteMedicalExpert(id);
        }

        public List<MedicalExpert> SearchDoctors(string searchTerm)
        {
            return _medicalExpertRepository.SearchDoctors(searchTerm);
        }

        public List<string> GetScheduleByExpertId(int expertId)
        {
            return _medicalExpertRepository.GetScheduleByExpertId(expertId);
        }


        public List<RatingsAndFeedback> GetFeedbacksByExpertId(int expertId)
        {
            return _medicalExpertRepository.GetFeedbacksByExpertId(expertId);
        }
        public async Task UpdateScheduleAsync(int expertId, List<string> selectedDays)
        {
            await _medicalExpertRepository.UpdateScheduleAsync(expertId, selectedDays);
        }
        public List<MedicalExpert> getExpertsByFacilityId(int facilityId)
        {
           return _medicalExpertRepository.getExpertsByFacilityId((int)facilityId);
        }
    }
}
