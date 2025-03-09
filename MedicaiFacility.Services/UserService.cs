using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User findById(int id)=> _userRepository.findById(id);
		
	}
}
