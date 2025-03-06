using Azure;
using MedicaiFacility.BusinessObject;
using MedicaiFacility.BusinessObject.Pagination;
using MedicaiFacility.DataAccess.IRepostory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _Context;
        public DepartmentRepository(AppDbContext context)
        {
            _Context = context;
        }
        public List<Department> GetAllDepartment()
        {
            return _Context.Departments
                //.OrderByDescending(d => d.DepartmentId)
                .ToList();
        }
        public Department FindById(int id)
        {
            return _Context.Departments.Find(id);
        }

        public void AddDepartment(Department department)
        {
            _Context.Departments.Add(department);
            _Context.SaveChanges();
        }

        public void UpdateDepartment(Department department)
        {
            _Context.Departments.Update(department);
            _Context.SaveChanges();
        }

        public void DeleteDepartment(int id)
        {
            throw new NotImplementedException();
        }

        public (List<Department>, int totalItem) FindAllWithPagination(int pg, int pageSize)
        {
            //var query = _Context.Departments
            //    .OrderByDescending(d => d.DepartmentId); 

            //int totalItem = query.Count();
            //var data = query
            //    .Skip((pg - 1) * pageSize)
            //.Take(pageSize)
            //.ToList();

            //return (data, totalItem);
            var query = _Context.Departments;
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
