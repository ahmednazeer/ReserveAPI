using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using MySQLIdentity.Dtos;
using MySQLIdentity.Models;
using MySQLIdentity.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MySQLIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserManager<User> _userManger;
        private SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        public UsersController(Microsoft.AspNetCore.Identity.UserManager<User> userManger, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManger = userManger;
            _signInManager = signInManager;
            _configuration = configuration;

        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(UserModel userModel)
        {
            var user = new User()
            {
                UserName = userModel.Name,
                Id=userModel.ID,
                Email= userModel.Email,
                PasswordHash= userModel.Password
            };
            try {
                var result = await _userManger.CreateAsync(user,userModel.Password);
                if (result == Microsoft.AspNetCore.Identity.IdentityResult.Success)
                {
                    return Ok("User Registered Successfully");
                }
                else 
                {
                    return BadRequest("Something wrong with your request");
                }
                //return Ok();
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        [HttpPost("login")]
        public async Task<object> Login( LoginDto model)
        {
            //var x = User.Identity.IsAuthenticated;
            
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManger.Users.SingleOrDefault(r => r.UserName == model.Email);
                var token= await GenerateJwtToken(model.Email, appUser);
                return token;
            }

            else
            {
                throw new ApplicationException("INVALID_LOGIN_ATTEMPT");

            }
        }



        /*
        [HttpGet("login")]
        public async Task<IActionResult> Login(string email,string password, bool rememberMe) {

            if (!email.Equals(null) && !password.Equals(null))
            {
                return await LoginByEmail(email, password);

            }
            else if(password==null)
            {
                return BadRequest("Insert Password");
            }
            else if(email==null) {
                return BadRequest("Insert Email");

            }
            else
            {
                return BadRequest("Insert Email and Password");

            }
        }
        */
        private async Task<IActionResult> LoginByEmail(string email,string password)
        {
            var user = await _userManger.FindByEmailAsync(email);
            return user == null ? BadRequest("Invalid Username or Password") : CheckPassword(user,password);
        }

        private async Task<IActionResult> LoginByUsername(string username, string password)
        {
            var user = await _userManger.FindByNameAsync(username);
            return user == null ? BadRequest("Invalid Username or Password") : CheckPassword(user, password);
        }

        private IActionResult CheckPassword(User user,string password)
        {
            
            if (user.PasswordHash== password)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid Username or Password");
            }


        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));//.ToString();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));


            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            ) ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}