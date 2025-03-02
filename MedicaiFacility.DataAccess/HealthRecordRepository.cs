using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.BusinessObject.Pagination;
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
        public (List<HealthRecord>, int totalItem) findAllWithPagination(int pg, int pageSize) {
            var healthRecords = _Context.HealthRecords
             .Include(hr => hr.Patient) // Include Patient
                 .ThenInclude(p => p.User) // Include User từ Patient
             .Include(hr => hr.UploadedByNavigation) // Include MedicalExpert
                 .ThenInclude(me => me.User) // Include User từ MedicalExpert
             .ToList();
            int totalItem = healthRecords.Count();
            var pager = new Pager(totalItem, pg, pageSize);
            int skipItem = (pg-1)*pageSize;
            var data = healthRecords.Skip(skipItem).Take(pager.Pagesize).ToList();
            return (data, totalItem);
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
