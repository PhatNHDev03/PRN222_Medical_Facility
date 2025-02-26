using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly AppDbContext _Context;
        public HealthRecordRepository(AppDbContext context)
        {
            _Context = context;
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
            var healthRecords = _Context.HealthRecords
            .Include(hr => hr.Patient) // Include Patient
                .ThenInclude(p => p.User) // Include User từ Patient
            .Include(hr => hr.UploadedByNavigation) // Include MedicalExpert
                .ThenInclude(me => me.User) // Include User từ MedicalExpert
            .ToList();
            return healthRecords;
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
