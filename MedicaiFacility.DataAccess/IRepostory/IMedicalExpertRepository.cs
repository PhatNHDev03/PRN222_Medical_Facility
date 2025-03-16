using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess.IRepostory
{
    public interface IMedicalExpertRepository
    {
        MedicalExpert getById(int id);
        List<MedicalExpert> SearchDoctors(string searchTerm);
    }
}
