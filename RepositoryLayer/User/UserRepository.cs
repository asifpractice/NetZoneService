using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NetZoneApplication.DataLayer.User;
using System.Data;
using System.Linq;

namespace NetZoneApplication.RepositoryLayer.User
{
    public class UserRepository : IUserRepository
    {
        IConfiguration config;
        public UserRepository(IConfiguration _config)
        {
            config = _config;
        }

        public IDbConnection connection => new SqlConnection(config.GetConnectionString("SqlConnString"));

        public UserEntity LoginValidate(string userName, string password)
        {
            UserEntity user = new UserEntity();
            using (IDbConnection conn = connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var param = new DynamicParameters();
                param.Add("@EmailID", userName);
                user = conn.Query<UserEntity>("Pr_NZ_ValidateLoginUser", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.Salt))
                return null;

            return user;
        }
        public IEnumerable<UserDTO> GetUserList()
        {
            IEnumerable<UserDTO> userList = new List<UserDTO>();
            using (IDbConnection conn = connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                userList = conn.Query<UserDTO>("Pr_NZ_GetUserList").ToList();
            }
            return userList;
        }

        public UserDTO GetUserById(int userId)
        {            
            using (IDbConnection conn = connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", userId);
                var userList = conn.Query<UserDTO>("Pr_NZ_GetUserDetailsById", param, commandType: CommandType.StoredProcedure).ToList();
                if (userList != null && userList.Count > 0)
                    return userList[0];
                else
                    return new UserDTO();
            }            
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public bool InsertupdateUserDetails(UserProfile user)
        {
            int ctr = 0;
            byte[] passwordHash, salt;
            this.CreatePasswordHash(user.Password, out passwordHash, out salt);
            using (IDbConnection conn = connection)
            {
                conn.Open();
                using (IDbTransaction transaction = conn.BeginTransaction())
                {
                    var temp = new DynamicParameters();
                    temp.Add("@UserID", user.UserID);
                    temp.Add("@FirstName", user.FirstName);
                    temp.Add("@LastName", user.LastName);
                    temp.Add("@EmailID", user.EmailID);
                    temp.Add("@Password", passwordHash);
                    temp.Add("@Salt", salt);                    
                    temp.Add("@ContactNo", user.ContactNo);

                    ctr = conn.Execute("Pr_NZ_InsertUpdateUserDetails", temp, transaction, 0, System.Data.CommandType.StoredProcedure);
                    if (ctr > 0)
                        transaction.Commit();
                    else
                        transaction.Rollback();                    
                }
            }
            return ctr > 0 ? true : false;
        }
    }
}
