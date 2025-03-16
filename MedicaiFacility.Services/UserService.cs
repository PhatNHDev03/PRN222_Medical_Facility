using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using System.Collections.Generic;

namespace MedicaiFacility.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User FindById(int id)
        {
            return _userRepository.FindById(id);
        }

        public User SignIn(string identifier, string password)
        {
            return _userRepository.ValidateLogin(identifier, password);
        }

        public void SignUp(User user)
        {
            _userRepository.RegisterUser(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user); 
        }
        public User FindByEmail(string email)
        {
            return _userRepository.FindByEmail(email);
        }

        public User IsExistEmail(string email)
        {
            return _userRepository.IsExistEmail(email);
        }

        public bool ValidatePassword(string email, string password)
        {
            var user = _userRepository.FindByEmail(email);
            if (user == null)
            {
                return false;
            }
            return user.Password == password;
        }

        public void ChangePassword(string email, string newPassword)
        {
            _userRepository.ChangePassword(email, newPassword);
        }
    }
}