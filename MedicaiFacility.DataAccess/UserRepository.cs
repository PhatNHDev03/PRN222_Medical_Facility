using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

		public User findById(int id) => _context.Users.FirstOrDefault(x => x.UserId == id);
		
	}
}
