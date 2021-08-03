using NetZoneApplication.DataLayer.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.User
{
    public interface IUserService
    {        
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<UserDTO> GetUserList();
        UserDTO GetUserDetailsById(int userId);
        bool InsertupdateUserDetails(UserProfile user);
    }
}
