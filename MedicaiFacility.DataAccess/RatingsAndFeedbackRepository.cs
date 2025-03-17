using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicaiFacility.DataAccess
{
    public class RatingsAndFeedbackRepository
    {
        private readonly AppDbContext _context;

        public RatingsAndFeedbackRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<RatingsAndFeedback> GetAll()
        {
            return _context.RatingsAndFeedbacks.ToList();
        }

        public RatingsAndFeedback GetById(int id)
        {
            return _context.RatingsAndFeedbacks.FirstOrDefault(r => r.FeedbackId == id);
        }

        public void Add(RatingsAndFeedback feedback)
        {
            _context.RatingsAndFeedbacks.Add(feedback);
            _context.SaveChanges();
        }

        public void Update(RatingsAndFeedback feedback)
        {
            _context.RatingsAndFeedbacks.Update(feedback);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var feedback = _context.RatingsAndFeedbacks.FirstOrDefault(r => r.FeedbackId == id);
            if (feedback != null)
            {
                _context.RatingsAndFeedbacks.Remove(feedback);
                _context.SaveChanges();
            }
        }
    }
}
