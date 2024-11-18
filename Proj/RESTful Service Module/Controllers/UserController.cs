using DBScaffold.Models;
using FIS_API.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RESTful_Service_Module.Dtos;

namespace RESTful_Service_Module.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IConfiguration _configuration;
        private readonly DwaContext _context;

        public UserController(IConfiguration configuration, DwaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // POST api/<LoginController>
        [HttpPost("login")]
        public ActionResult LogIn(LoginInDto loginData)
        {
            loginData.Username = loginData.Username.Trim();
            loginData.Password = loginData.Password.Trim();

            try
            {
                const string genericLoginFail = "Incorrect username or password";

                // Try to get a user from database
                var login = _context.Logins.Include(x => x.Users).FirstOrDefault(x => x.Email == loginData.Username);
                var adminList = _context.Administrators;

                if (login == null)
                    return BadRequest(genericLoginFail);

                var user = login.Users.FirstOrDefault();

                if (user == null)
                    return BadRequest(genericLoginFail);

                // Check the password
                if (loginData.Password != login.PasswordPlain)
                    return BadRequest(genericLoginFail);

                // Create and return JWT token
                var secureKey = _configuration["JWT:SecureKey"];

                string role;
                if (adminList.FirstOrDefault(x => x.UserId == user.Iduser) == null)
                    role = "User";
                else
                    role = "Administrator";

                var serializedToken =
                    JwtTokenProvider.CreateToken(
                        secureKey,
                        3600,
                        loginData.Username,
                        role);

                return Ok(serializedToken);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
