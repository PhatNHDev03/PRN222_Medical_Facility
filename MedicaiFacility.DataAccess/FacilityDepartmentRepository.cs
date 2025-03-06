using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess
{
    public class FacilityDepartmentRepository : IFacilityDepartmentRepository
    {
        private readonly AppDbContext _Context;
        public FacilityDepartmentRepository(AppDbContext context)
        {
            _Context = context;
        }
        public List<FacilityDepartment> GetAllFacilityDepartment()
        {
            return _Context.FacilityDepartments.ToList();
        }
        public FacilityDepartment FindById(int id)
        {
            throw new NotImplementedException();
        }

        public void AddFacilityDepartment(FacilityDepartment facilityDepartment)
        {
            _Context.FacilityDepartments.Add(facilityDepartment);
            _Context.SaveChanges();
        }

        public void UpdateFacilityDepartment(FacilityDepartment facilityDepartment)
        {
            throw new NotImplementedException();
        }

        public void DeleteFacilityDepartment(int id)
        {
            throw new NotImplementedException();
        }

        public (List<FacilityDepartment>, int totalItem) FindAllWithPagination(int pg, int pageSize)
        {
            var query = _Context.FacilityDepartments;
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
