using MedicaiFacility.BusinessObject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess.IRepostory
{
    public interface IHealthRecordRepository
    {
        public List<HealthRecord> GetAll();
        public HealthRecord FindById(int id);
        public void Save();
        public void Udpate ();
        public void deleteById(int id);
    }
}
