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
    public class MedicalHistoryService : IMedicalHistoryService
    {
        private readonly IMedicalHistoryRepository _medicalHistoryRepository;

        public MedicalHistoryService(IMedicalHistoryRepository medicalHistoryRepository)
        {
            _medicalHistoryRepository = medicalHistoryRepository;
        }

        public void Create(MedicalHistory medicalHistory)
        {
            _medicalHistoryRepository.Create(medicalHistory);  
        }

        public void DeleteById(int id)
        {
            _medicalHistoryRepository.DeleteById(id);
        }

        public List<MedicalHistory> GetAll()
        {
         return _medicalHistoryRepository.GetAll();
        }

        public (List<MedicalHistory> list, int totalItems) GetALlPagainations(int pg, int pageSize)
        {
            return _medicalHistoryRepository.GetALlPagainations(pg, pageSize);
        }

        public MedicalHistory GetById(int id)
        {
            return _medicalHistoryRepository.GetById(id);
        }

        public void Update(MedicalHistory medicalHistory)
        {
            _medicalHistoryRepository.Update(medicalHistory);   
        }
    }
}
