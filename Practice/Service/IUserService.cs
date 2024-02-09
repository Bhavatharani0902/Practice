using Practice.Entities;
using System.Collections.Generic;

namespace Practice.Service
{
    public interface IUserService
    {
        void CreateUser(User user);

        void DeleteUser(int userId);

        void EditUser(User user);

        List<User> GetAllUsers();

        User GetUser(int userId);

        User ValidateUser(string email, string password);

        User GetUserById(int userId);
    }
}
