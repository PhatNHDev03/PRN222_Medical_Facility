using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess.IRepostory
{
    public interface ISystemBalanceRepository
    {
        void update(int SystemId, decimal amount);
    }
}
