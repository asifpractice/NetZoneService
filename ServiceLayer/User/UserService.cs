using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetZoneApplication.BaseEntity;
using NetZoneApplication.DataLayer.User;
using NetZoneApplication.RepositoryLayer.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.User
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private readonly AppSettings appSettings;

        public UserService(IUserRepository _userRepository, IOptions<AppSettings> _appSettings)
        {
            this.userRepository = _userRepository;
            this.appSettings = _appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {            
            var user = userRepository.LoginValidate(model.Username, model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<UserDTO> GetUserList()
        {
            IEnumerable<UserDTO> userList = userRepository.GetUserList();
            return userList;
        }

        public UserDTO GetUserDetailsById(int userId)
        {
            UserDTO user = userRepository.GetUserById(userId);
            return user;
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var claims = new[]
            {
                new Claim("UserID", user.UserID.ToString()),
                new Claim("UserEmail", user.EmailID),
                new Claim("UserName", string.Format("{0} {1}",user.FirstName, user.LastName))
            };
                        
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);            
        }

        public bool InsertupdateUserDetails(UserProfile user)
        {
            return userRepository.InsertupdateUserDetails(user);
        }
    }
}
