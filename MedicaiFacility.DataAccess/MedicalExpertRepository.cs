using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _context.MedicalExperts
                .Include(x => x.Facility)
                .Include(x => x.Expert)
                .Include(x => x.MedicalExpertSchedules)
                .FirstOrDefault(x => x.ExpertId == id);
        }

        public void Add(MedicalExpert medicalExpert)
        {
            Console.WriteLine($"Adding MedicalExpert with ExpertId: {medicalExpert.ExpertId}");
            try
            {
                _context.MedicalExperts.Add(medicalExpert);
                _context.SaveChanges();
                Console.WriteLine($"MedicalExpert saved to database with ExpertId: {medicalExpert.ExpertId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving MedicalExpert: {ex.Message} - InnerException: {ex.InnerException?.Message}");
                throw;
            }
        }

        public IEnumerable<MedicalExpert> GetAllMedicalExperts()
        {
            var experts = _context.MedicalExperts
                .Include(x => x.Facility)
                .Include(x => x.Expert)
                .Include(x => x.MedicalExpertSchedules)
                .ToList();
            Console.WriteLine($"GetAllMedicalExperts: Loaded {experts.Count} experts.");
            return experts;
        }

        public void Update(MedicalExpert medicalExpert)
        {
            var existingMedicalExpert = _context.MedicalExperts.FirstOrDefault(x => x.ExpertId == medicalExpert.ExpertId);
            if (existingMedicalExpert == null)
            {
                throw new Exception("Medical Expert not found!");
            }

            existingMedicalExpert.Specialization = medicalExpert.Specialization;
            existingMedicalExpert.ExperienceYears = medicalExpert.ExperienceYears;
            existingMedicalExpert.Department = medicalExpert.Department;
            existingMedicalExpert.PriceBooking = medicalExpert.PriceBooking;
            existingMedicalExpert.FacilityId = medicalExpert.FacilityId;

            _context.Entry(existingMedicalExpert).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var medicalExpert = _context.MedicalExperts
                        .Include(x => x.Expert)
                        .FirstOrDefault(x => x.ExpertId == id);

                    if (medicalExpert == null)
                    {
                        transaction.Rollback();
                        return;
                    }

                    if (medicalExpert.Expert != null)
                    {
                        medicalExpert.Expert = null;
                        _context.Entry(medicalExpert.Expert).State = EntityState.Modified;
                    }

                    _context.MedicalExperts.Remove(medicalExpert);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Failed to delete medical expert with ID {id}: {ex.Message}", ex);
                }
            }
        }

        public void AddMedicalExpertSchedule(MedicalExpertSchedule schedule)
        {
            Console.WriteLine($"Attempting to add schedule: ExpertId={schedule.ExpertId}, Day={schedule.DayOfWeek}");
            try
            {
                _context.MedicalExpertSchedules.Add(schedule);
                _context.SaveChanges();
                Console.WriteLine($"Schedule saved successfully with ScheduleId: {schedule.ScheduleId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving schedule: {ex.Message} - InnerException: {ex.InnerException?.Message}");
                throw;
            }
        }

        public void DeleteSchedulesByExpertId(int expertId)
        {
            Console.WriteLine($"Attempting to delete schedules for ExpertId: {expertId}");
            try
            {
                var schedules = _context.MedicalExpertSchedules
                    .Where(s => s.ExpertId == expertId)
                    .ToList();

                if (schedules.Any())
                {
                    _context.MedicalExpertSchedules.RemoveRange(schedules);
                    _context.SaveChanges();
                    Console.WriteLine($"Successfully deleted {schedules.Count} schedules for ExpertId: {expertId}");
                }
                else
                {
                    Console.WriteLine($"No schedules found for ExpertId: {expertId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting schedules for ExpertId {expertId}: {ex.Message} - InnerException: {ex.InnerException?.Message}");
                throw;
            }
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
                    me.Department.ToLower().Contains(searchTerm) ||
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

        public List<MedicalExpert> getExpertsByFacilityId(int facilityId)
        {
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