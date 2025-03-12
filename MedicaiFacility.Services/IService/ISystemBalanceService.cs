using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service.IService
{
    public interface ISystemBalanceService
    {
        void update(int SystemId, decimal amount);
    }
}
