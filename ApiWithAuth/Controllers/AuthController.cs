using ApiWithAuth.Entities;
using ApiWithAuth.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace ApiWithAuth.Controllers
{
    public class AuthController : Controller
    {
        //private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly JwtDemoContext _context;

        public AuthController(TokenService tokenService, JwtDemoContext context)
        {
            //_userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            var userInDb = _context.users.SingleOrDefault(user => user.Username == request.Email && user.Password == request.Password);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(userInDb == null)
            {
                return BadRequest("Invalid username or password");
            }
            else
            {
                var user = new ApplicationUser { Email = userInDb.Username, UserName = userInDb.Username, Id = Guid.NewGuid().ToString(), Role = userInDb.Role };
                var accesstoken = _tokenService.CreateToken(user);

                return Ok(new AuthResponse
                {
                    Username = userInDb.Username,
                    Role = userInDb.Role,
                    Token = accesstoken
                });
            }

        }
    }
}
