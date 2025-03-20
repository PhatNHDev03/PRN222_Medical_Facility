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
            return _context.MedicalExperts.Include(x=>x.MedicalExpertSchedules).Include(x=>x.Facility).Include(x=>x.Expert).FirstOrDefault(x => x.ExpertId == id);
        }
        public List<MedicalExpert> SearchDoctors(string searchTerm)
        {
            searchTerm = searchTerm?.ToLower();

            var query = _context.MedicalExperts
                .Include(me => me.Expert)
                .Include(me => me.Facility)
                .Include(me => me.MedicalExpertSchedules)
                .Where(me =>
                    me.Expert != null && me.Expert.Status == true &&
                    me.MedicalExpertSchedules.Any() &&
                    (string.IsNullOrEmpty(searchTerm) ||
                    me.Expert.FullName.ToLower().Contains(searchTerm) ||
                    me.Specialization.ToLower().Contains(searchTerm) ||
                    me.ExperienceYears.ToString().Contains(searchTerm) ||
                    me.Facility.FacilityName.ToLower().Contains(searchTerm) ||
                    me.Facility.Address.ToLower().Contains(searchTerm) ||
                    me.PriceBooking.ToString().Contains(searchTerm)));

            return query.ToList();
        }
        public List<string> GetScheduleByExpertId(int expertId)
        {
            return _context.MedicalExpertSchedules
                .Where(s => s.ExpertId == expertId)
                .Select(s => s.DayOfWeek)
                .ToList();
        }
        public List<RatingsAndFeedback> GetFeedbacksByExpertId(int expertId)
        {
            return _context.RatingsAndFeedbacks
                .Include(rf => rf.MedicalHistory)
                .ThenInclude(mh => mh.Appointment)
                .Where(rf => rf.MedicalHistory != null && rf.MedicalHistory.Appointment != null && rf.MedicalHistory.Appointment.ExpertId == expertId)
                .ToList();
        }
    }
}
