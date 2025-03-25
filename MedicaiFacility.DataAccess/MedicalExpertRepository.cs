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
        public async Task<MedicalExpert> getByIdAsync(int id)
        {
            return await _context.MedicalExperts
                .Include(x => x.MedicalExpertSchedules)
                .Include(x => x.Facility)
                .Include(x => x.Expert)
                .FirstOrDefaultAsync(x => x.ExpertId == id);
        }
        public List<MedicalExpert> GetAllMedicalExpert()
        {
            return _context.MedicalExperts
                .Include(me => me.Expert)
                .Include(me => me.MedicalExpertSchedules)
                .Include(me => me.Facility)
                .ToList();
        }

        //public void AddMedicalExpert(MedicalExpert medicalExpert)
        //{
        //    _context.MedicalExperts.Add(medicalExpert);
        //    _context.SaveChanges();
        //}

        public void UpdateMedicalExpert(MedicalExpert medicalExpert)
        {
            _context.MedicalExperts.Update(medicalExpert);
            _context.SaveChanges();
        }
        public void DeleteMedicalExpert(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var medicalExpert = _context.MedicalExperts
                    .FirstOrDefault(me => me.ExpertId == id);

                if (medicalExpert == null)
                {
                    throw new InvalidOperationException($"MedicalExpert with ID {id} not found.");
                }

                var user = _context.Users
                    .FirstOrDefault(u => u.UserId == id);

                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {id} not found.");
                }

                user.Status = false;
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Failed to delete medicalExpert and update associated experts: {ex.Message}", ex);
            }
        }
        public MedicalExpert getById(int id)
        {
            return _context.MedicalExperts.Include(x=>x.MedicalExpertSchedules).Include(x=>x.Facility).Include(x=>x.Expert).FirstOrDefault(x => x.ExpertId == id);
        }
        public (List<MedicalExpert>, int totalItem) FindAllWithPagination(int pg, int pageSize)
        {
            var query = _context.MedicalExperts
                 .Include(me => me.Expert)
                 .Include(me => me.MedicalExpertSchedules)
                 .Include(me => me.Facility);

            int totalItem = query.Count();
            var data = query
                .Skip((pg - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (data, totalItem);
        }
        public List<MedicalExpert> SearchDoctors(string searchTerm)
        {
            searchTerm = searchTerm?.ToLower();

            var query = _context.MedicalExperts
                .Include(me => me.Expert)
                .Include(me => me.Facility)
                .Include(me => me.MedicalExpertSchedules)
                .Where(me =>
                    me.Expert != null && me.Expert.Status == true && me.MedicalExpertSchedules.Any() &&
                    (string.IsNullOrEmpty(searchTerm) ||
                    me.Expert.FullName.ToLower().Contains(searchTerm) ||
                    me.Specialization.ToLower().Contains(searchTerm) ||
                    me.StartHour.ToString().Contains(searchTerm) ||
                    me.EndHour.ToString().Contains(searchTerm) ||
                    me.Department.ToLower().Contains(searchTerm)|| 
                    me.ExperienceYears.ToString().Contains(searchTerm) ||
                    me.Facility.FacilityName.ToLower().Contains(searchTerm) ||
                    me.Facility.Address.ToLower().Contains(searchTerm) ||
                    me.Facility.Address.ToLower().Contains(searchTerm) ||
                    me.Facility.Address.ToLower().Contains(searchTerm) ||
                    me.PriceBooking.ToString().Contains(searchTerm)));

            return query.ToList();
        }
        public async Task UpdateScheduleAsync(int expertId, List<string> selectedDays)
        {
            var existingSchedules = _context.MedicalExpertSchedules
                .Where(s => s.ExpertId == expertId)
                .ToList();
            _context.MedicalExpertSchedules.RemoveRange(existingSchedules);

            foreach (var day in selectedDays)
            {
                _context.MedicalExpertSchedules.Add(new MedicalExpertSchedule
                {
                    ExpertId = expertId,
                    DayOfWeek = day
                });
            }

            await _context.SaveChangesAsync();
        }
        public List<string> GetScheduleByExpertId(int expertId)
        {
            return _context.MedicalExpertSchedules
                .Where(s => s.ExpertId == expertId)
                .Select(s => s.DayOfWeek)
                .ToList();
        }

        public List<MedicalExpert> getExpertsByFacilityId(int facilityId) {
            return _context.MedicalExperts.Where(x => x.FacilityId == facilityId).Include(x => x.Expert).Include(x => x.MedicalExpertSchedules).ToList();
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
