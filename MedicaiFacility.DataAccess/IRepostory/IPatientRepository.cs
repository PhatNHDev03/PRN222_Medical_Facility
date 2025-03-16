using MedicaiFacility.BusinessObject;
using System.Collections.Generic;

namespace MedicaiFacility.DataAccess.IRepostory
{
    public interface IPatientRepository
    {
        void Add(Patient patient);
        Patient FindById(int id);
        IEnumerable<Patient> GetAllPatients();
        void Update(Patient patient);
        void Delete(int id);
    }
}