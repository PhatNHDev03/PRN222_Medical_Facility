using MedicaiFacility.BusinessObject;
using System.Collections.Generic;

namespace MedicaiFacility.Service.IService
{
    public interface IPatientService
    {
        void CreatePatient(Patient patient);
        Patient GetPatientById(int id);
        IEnumerable<Patient> GetAllPatients();
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
    }
}