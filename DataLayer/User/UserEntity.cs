using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetZoneApplication.DataLayer.User
{
    public class UserEntity
    {
        public Int32 UserID { get; set; }
        public string EmailID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public Guid UserKey { get; set; }
        public string ContactNo { get; set; }
        public bool Status { get; set; }
        public bool IsDel { get; set; }
    }

    public class UserProfile
    {
        public Int32 UserID { get; set; }
        public string EmailID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }        
        public string ContactNo { get; set; }
    }

    public class UserDTO
    {
        public Int32 UserID { get; set; }
        public string EmailID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public bool Status { get; set; }
        public bool IsDel { get; set; }
    }
}
