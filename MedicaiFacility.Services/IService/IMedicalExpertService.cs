using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service.IService
{
    public interface IMedicalExpertService
    {
        MedicalExpert getById(int id);
        List<MedicalExpert> SearchDoctors(string searchTerm);
        List<string> GetScheduleByExpertId(int expertId);
        List<RatingsAndFeedback> GetFeedbacksByExpertId(int expertId);
    }
}
