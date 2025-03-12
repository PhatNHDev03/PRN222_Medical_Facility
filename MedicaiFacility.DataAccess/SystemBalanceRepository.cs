using MedicaiFacility.DataAccess.IRepostory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess
{
    public class SystemBalanceRepository : ISystemBalanceRepository
    {
        private readonly AppDbContext _context;
        public SystemBalanceRepository(AppDbContext context)
        {
            _context=context;
        }
        public void update(int SystemId, decimal amount)
        {
            var system = _context.SystemBalances.FirstOrDefault(x=>x.BalanceId == SystemId);
            system.TotalBalance += amount;
            _context.SystemBalances.Update(system);
            _context.SaveChanges();
        }
    }
}
