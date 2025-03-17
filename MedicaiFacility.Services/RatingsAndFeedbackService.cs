using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess;
using System.Collections.Generic;

namespace MedicaiFacility.Services
{
    public class RatingsAndFeedbackService
    {
        private readonly RatingsAndFeedbackRepository _repository;

        public RatingsAndFeedbackService(RatingsAndFeedbackRepository repository)
        {
            _repository = repository;
        }

        public List<RatingsAndFeedback> GetAllFeedbacks()
        {
            return _repository.GetAll();
        }

        public RatingsAndFeedback GetFeedbackById(int id)
        {
            return _repository.GetById(id);
        }

        public void AddFeedback(RatingsAndFeedback feedback)
        {
            _repository.Add(feedback);
        }

        public void UpdateFeedback(RatingsAndFeedback feedback)
        {
            _repository.Update(feedback);
        }

        public void DeleteFeedback(int id)
        {
            _repository.Delete(id);
        }
    }
}
