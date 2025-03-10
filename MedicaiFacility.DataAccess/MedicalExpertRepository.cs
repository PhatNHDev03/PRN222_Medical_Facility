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
    public class MedicalExpertRepository : IMedicalExpertRepository
    {
        private readonly AppDbContext _context;
    
        public MedicalExpertRepository(AppDbContext context) { 
            _context = context; 
        }    
        public MedicalExpert getById(int id)
        {
            return _context.MedicalExperts.Include(x=>x.Facility).Include(x=>x.Expert).FirstOrDefault(x => x.ExpertId == id);
        }
    }
}
