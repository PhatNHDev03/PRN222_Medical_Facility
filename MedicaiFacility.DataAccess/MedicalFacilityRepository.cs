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
        public (List<MedicalFacility>, Dictionary<int, List<string>>, int totalItem) FindAllWithDepartmentsAndPagination(int pg, int pageSize)
        {
            // Step 1: Fetch paginated medical facilities
            var facilitiesQuery = _Context.MedicalFacilities;
            int totalItem = facilitiesQuery.Count();

            var facilities = facilitiesQuery
                .Skip((pg - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Step 2: Fetch department names for the paginated facilities
            var facilityIds = facilities.Select(f => f.FacilityId).ToList();

            // Join FacilityDepartments with Departments to get department names
            var facilityDepartments = (from fd in _Context.FacilityDepartments
                                       join d in _Context.Departments on fd.DepartmentId equals d.DepartmentId
                                       where facilityIds.Contains(fd.FacilityId ?? 0)
                                       select new
                                       {
                                           fd.FacilityId,
                                           d.DepartmentName
                                       })
                                       .ToList();

            // Step 3: Group department names by FacilityId
            var facilityDepartmentsDict = facilityDepartments
                .GroupBy(fd => fd.FacilityId ?? 0)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(fd => fd.DepartmentName).ToList()
                );

            // Ensure all facilities have an entry in the dictionary, even if they have no departments
            foreach (var facility in facilities)
            {
                if (!facilityDepartmentsDict.ContainsKey(facility.FacilityId))
                {
                    facilityDepartmentsDict[facility.FacilityId] = new List<string>();
                }
            }

            return (facilities, facilityDepartmentsDict, totalItem);
        }
    }
}
