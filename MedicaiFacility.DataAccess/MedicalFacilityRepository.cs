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
    public class MedicalFacilityRepository : IMedicalFacilityRepository
    {
        private readonly AppDbContext _Context;
        public MedicalFacilityRepository(AppDbContext context)
        {
            _Context = context;
        }
        public List<MedicalFacility> GetAllMedicalFacility()
        {
            return _Context.MedicalFacilities.ToList();
        }
        public MedicalFacility FindById(int id)
        {
            return _Context.MedicalFacilities.Find(id);
        }

        public void AddMedicalFacility(MedicalFacility medicalFacility)
        {
            _Context.MedicalFacilities.Add(medicalFacility);
            _Context.SaveChanges();
        }

        public void UpdateMedicalFacility(MedicalFacility medicalFacility)
        {
            _Context.MedicalFacilities.Update(medicalFacility);
            _Context.SaveChanges();
        }

        public void DeleteMedicalFacility(int id)
        {
            var medicalFacility = _Context.MedicalFacilities.Find(id);
            if (medicalFacility == null)
            {
                throw new InvalidOperationException($"MedicalFacility with ID {id} not found.");
            }
            medicalFacility.IsActive = false;
            _Context.MedicalFacilities.Update(medicalFacility);
            _Context.SaveChanges();
        }

        public (List<MedicalFacility>, int totalItem) FindAllWithPagination(int pg, int pageSize)
        {
            var query = _Context.MedicalFacilities;
            //.OrderByDescending(d => d.DepartmentId); 

            int totalItem = query.Count();
            var data = query
                .Skip((pg - 1) * pageSize) // Skip records for previous pages
                .Take(pageSize) // Take only the current page's worth
                .ToList();

            return (data, totalItem);
        }
    }
}
