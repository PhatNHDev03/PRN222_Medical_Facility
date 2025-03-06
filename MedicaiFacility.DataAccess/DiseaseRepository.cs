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
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly AppDbContext _Context;
        public DiseaseRepository(AppDbContext context)
        {
            _Context = context;
        }
        public List<Disease> GetAllDisease()
        {
            return _Context.Diseases.Include(d => d.Department).ToList();
        }
        public Disease FindById(int id)
        {
            throw new NotImplementedException();
        }

        public void AddDisease(Disease disease)
        {
            _Context.Diseases.Add(disease);
            _Context.SaveChanges();
        }

        public void UpdateDisease(Disease disease)
        {
            throw new NotImplementedException();
        }

        public void DeleteDisease(int id)
        {
            throw new NotImplementedException();
        }

        public (List<Disease>, int totalItem) FindAllWithPagination(int pg, int pageSize)
        {
            var query = _Context.Diseases.Include(d => d.Department);
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
