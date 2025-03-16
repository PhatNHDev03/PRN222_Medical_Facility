using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using System;

namespace MedicaiFacility.Service
{
    public class MedicalExpertService : IMedicalExpertService
    {
        private readonly IMedicalExpertRepository _medicalExpertRepository;
        private readonly IUserRepository _userRepository;

        public MedicalExpertService(IMedicalExpertRepository medicalExpertRepository, IUserRepository userRepository)
        {
            _medicalExpertRepository = medicalExpertRepository ?? throw new ArgumentNullException(nameof(medicalExpertRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public MedicalExpert getById(int id)
        {
            return _medicalExpertRepository.getById(id);
        }

        public void CreateMedicalExpert(MedicalExpert medicalExpert)
        {
            Console.WriteLine($"Creating MedicalExpert with ExpertId: {medicalExpert.ExpertId}");
            var user = _userRepository.FindById(medicalExpert.ExpertId);
            if (user == null)
            {
                Console.WriteLine("User not found!");
                throw new Exception("User not found!");
            }

            if (user.UserType != "MedicalExpert")
            {
                Console.WriteLine("User is not a MedicalExpert!");
                throw new Exception("Selected user is not a medical expert!");
            }

            try
            {
                _medicalExpertRepository.Add(medicalExpert);
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
            return _medicalExpertRepository.GetAllMedicalExperts();
        }

        public void UpdateMedicalExpert(MedicalExpert medicalExpert)
        {
            _medicalExpertRepository.Update(medicalExpert);
        }

        public void DeleteMedicalExpert(int id)
        {
            try
            {
                _medicalExpertRepository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete medical expert with ID {id}: {ex.Message}");
                throw new Exception($"Failed to delete medical expert with ID {id}: {ex.Message}", ex);
            }
        }

        public void AddMedicalExpertSchedule(MedicalExpertSchedule schedule)
        {
            Console.WriteLine($"Adding schedule for ExpertId: {schedule.ExpertId}, Day: {schedule.DayOfWeek}");
            _medicalExpertRepository.AddMedicalExpertSchedule(schedule);
            Console.WriteLine("Schedule added successfully.");
        }

        public void DeleteSchedulesByExpertId(int expertId)
        {
            Console.WriteLine($"Deleting schedules for ExpertId: {expertId}");
            _medicalExpertRepository.DeleteSchedulesByExpertId(expertId);
            Console.WriteLine($"Schedules deleted for ExpertId: {expertId}");
        }
    }
}