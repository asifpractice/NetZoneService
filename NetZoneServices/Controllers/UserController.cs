using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.User;
using Microsoft.Extensions.Configuration;
using NetZoneApplication.DataLayer.User;

namespace NetZoneServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userservice;
        private readonly IConfiguration config;

        public UserController(IUserService userservice, IConfiguration _config)
        {
            this.userservice = userservice;
            this.config = _config;
        }        

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult AuthenticateUser(AuthenticateRequest model)
        {
            var response = userservice.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("getuserlist")]
        public IActionResult GetUserList()
        {
            var users = userservice.GetUserList();
            return Ok(users);
        }

        //[Authorize]
        [HttpPost("saveuserdetails")]
        public IActionResult SaveUserProfile([FromBody] UserProfile user)
        {
            var result = userservice.InsertupdateUserDetails(user);
            return Ok(result);
        }
    }
}
