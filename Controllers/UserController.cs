using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using webAPI.JWTTOken.Models;

namespace webAPI.JWTTOken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint() 
        {
            var CurrentUser = GetCurrentUser();
            return Ok($"Hi you are an {CurrentUser.Role}");
        }

        private UserModel GetCurrentUser() {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!=null)
            {
                var usersClaims = identity.Claims;
                return new UserModel
                {
                    Username = usersClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = usersClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };


            }
            return null;
        }
    }
}
