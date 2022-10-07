using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webAPI.JWTTOken.Models;

namespace webAPI.JWTTOken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromForm] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound("User Not Found");
        }
        //[HttpPost,Route("login")]
        //public IActionResult Login(UserLogin loginDTO)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(loginDTO.Username) ||
        //        string.IsNullOrEmpty(loginDTO.Password))
        //            return BadRequest("Username and/or Password not specified");
        //        if (loginDTO.Username.Equals("Test") &&
        //        loginDTO.Password.Equals("Test"))
        //        {
        //            var secretKey = new SymmetricSecurityKey
        //            (Encoding.UTF8.GetBytes("thisisasecretkey@123"));
        //            var signinCredentials = new SigningCredentials
        //           (secretKey, SecurityAlgorithms.HmacSha256);
        //            var jwtSecurityToken = new JwtSecurityToken(
        //                issuer: "ABCXYZ",
        //                audience: "http://localhost:51398",
        //                claims: new List<Claim>(),
        //                expires: DateTime.Now.AddMinutes(10),
        //                signingCredentials: signinCredentials
        //            );
        //            Ok(new JwtSecurityTokenHandler().
        //            WriteToken(jwtSecurityToken));
        //        }
        //    }
        //    catch
        //    {
        //        return BadRequest
        //        ("An error occurred in generating the token");
        //    }
        //    return Unauthorized();
        //}
        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = UserConstant.Users.FirstOrDefault(x => x.Username.ToLower() == userLogin.Username.ToLower() && x.Password == userLogin.Password);
            if (currentUser != null)
            {
                return currentUser;

            }
            return null;
        }

        private string GenerateToken(UserModel user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {

            new Claim(ClaimTypes.NameIdentifier,user.Username),
            new Claim(ClaimTypes.Role,user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
