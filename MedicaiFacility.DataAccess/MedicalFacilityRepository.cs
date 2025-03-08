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
            using var transaction = _Context.Database.BeginTransaction();
            try
            {
                var medicalFacility = _Context.MedicalFacilities.Find(id);
                if (medicalFacility == null)
                {
                    throw new InvalidOperationException($"MedicalFacilities with ID {id} not found.");
                }

                // Delete Ref in MedicalExperts
                var relatedMedicalExperts = _Context.MedicalExperts
                    .Where(me => me.FacilityId == id)
                    .ToList();
                if (relatedMedicalExperts.Any())
                {
                    _Context.MedicalExperts.RemoveRange(relatedMedicalExperts);
                }

                // Delete Ref in Appointments
                var relatedAppointments = _Context.Appointments
                    .Where(a => a.FacilityId == id)
                    .ToList();
                if (relatedAppointments.Any())
                {
                    _Context.Appointments.RemoveRange(relatedAppointments);
                }

                // set id false in FacilityDepartments
                var relatedFacilityDepartments = _Context.FacilityDepartments
                    .Where(fd => fd.FacilityId == id)
                    .ToList();
                if (relatedFacilityDepartments.Any())
                {
                    foreach (var fd in relatedFacilityDepartments)
                    {
                        fd.Status = false; // Update the status field
                    }
                    _Context.FacilityDepartments.UpdateRange(relatedFacilityDepartments);
                }

                // delete facility
                _Context.MedicalFacilities.Remove(medicalFacility);
                _Context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Failed to delete Medical Facility with ID {id}: {ex.Message}", ex);
            }
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
