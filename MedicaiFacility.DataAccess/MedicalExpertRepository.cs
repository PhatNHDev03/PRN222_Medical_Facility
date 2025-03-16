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
        public List<MedicalExpert> SearchDoctors(string searchTerm)
        {
            searchTerm = searchTerm?.ToLower();

            var query = _context.MedicalExperts
                .Include(me => me.Expert)
                .Include(me => me.Facility)
                .Where(me => string.IsNullOrEmpty(searchTerm) ||
                             me.Expert.FullName.ToLower().Contains(searchTerm) ||
                             me.Specialization.ToLower().Contains(searchTerm) ||
                             me.ExperienceYears.ToString().Contains(searchTerm) ||
                             me.Facility.FacilityName.ToLower().Contains(searchTerm) ||
                             me.Facility.Address.ToLower().Contains(searchTerm) ||
                             me.PriceBooking.ToString().Contains(searchTerm));

            return query.ToList();
        }
    }
}
