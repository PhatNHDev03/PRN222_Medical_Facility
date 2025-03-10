using MedicaiFacility.BusinessObject;
using System.Collections.Generic;

namespace MedicaiFacility.Service.IService
{
    public interface IUserService
    {
        User FindById(int id);
        User SignIn(string identifier, string password);
        void SignUp(User user);
        IEnumerable<User> GetAllUsers();
        void UpdateUser(User user); // Thêm phương thức này
    }
}