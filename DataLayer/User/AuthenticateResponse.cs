using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetZoneApplication.DataLayer.User
{
    public class AuthenticateResponse
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(UserEntity user, string token)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.EmailID;
            Token = token;
        }
    }
}
