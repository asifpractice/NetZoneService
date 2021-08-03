using NetZoneApplication.DataLayer.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetZoneApplication.RepositoryLayer.User
{
    public interface IUserRepository
    {
        UserEntity LoginValidate(string userName, string password);
        IEnumerable<UserDTO> GetUserList();
        UserDTO GetUserById(int userId);
        bool InsertupdateUserDetails(UserProfile user);
        //Roles GetRoles();

    }
}
