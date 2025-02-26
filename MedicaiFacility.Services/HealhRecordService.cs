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
    public class HealhRecordService:IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        public HealhRecordService(IHealthRecordRepository healthRecordRepository)
        {
            _healthRecordRepository = healthRecordRepository;
        }

        public void deleteById(int id)
        {
            throw new NotImplementedException();
        }

        public HealthRecord FindById(int id)
        {
            throw new NotImplementedException();
        }

        public List<HealthRecord> GetAll()
        {
           return _healthRecordRepository.GetAll();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Udpate()
        {
            throw new NotImplementedException();
        }
    }
}
