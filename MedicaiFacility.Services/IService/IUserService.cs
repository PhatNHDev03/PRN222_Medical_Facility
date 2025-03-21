using MedicaiFacility.BusinessObject;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicaiFacility.Service.IService
{
    public interface IUserService
    {
        User FindById(int id);
        User SignIn(string identifier, string password);
        void SignUp(User user);
        IEnumerable<User> GetAllUsers();
        void UpdateUser(User user);
        Task UpdateUserAndSessionAsync(User user, HttpContext httpContext); // Thêm phương thức này
        User FindByEmail(string email);
        User IsExistEmail(string email);
        bool ValidatePassword(string email, string password);
        void ChangePassword(string email, string newPassword);
    }
}