using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private DBContext context;
        private readonly IAuthenticationSchemeProvider authenticationSchemeProvider;

 //       private readonly AccountService _accountService;
        //private string Token { set; get; }

        public UsersController(Microsoft.AspNetCore.Identity.UserManager<User> userManger, 
            SignInManager<User> signInManager,
            IConfiguration configuration,
            DBContext context,
            IAuthenticationSchemeProvider authenticationSchemeProvider
            //,AccountService _accountService
            )
        {
            _userManger = userManger;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.authenticationSchemeProvider = authenticationSchemeProvider;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(UserModel userModel)
        {
            var user = new User()
            {
                UserName = userModel.Email,
                Id=userModel.ID,
                Email= userModel.Email,
                PasswordHash= userModel.Password,
                FirstName=userModel.FirstName,
                LastName=userModel.LastName,
                Date_of_Birth=userModel.Date_of_Birth
            };
            try {
                var result = await _userManger.CreateAsync(user,userModel.Password);
                if (result == Microsoft.AspNetCore.Identity.IdentityResult.Success)
                {
                    return Ok(new { status = "Success" });
                }
                else 
                {
                    return BadRequest(new { status = "Fail" });
                }
                //return Ok();
            }
            catch (Exception e) {
                return BadRequest(new { status = "Fail",message= e.Message });
            }

        }

        [HttpPost("login")]
        public async Task<object> Login( LoginDto model)
        {
            //var x = User.Identity.IsAuthenticated;
            
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManger.Users.Include("Reservations").SingleOrDefault(r => r.UserName == model.Email);
                var token= GenerateJwtToken(model.Email, appUser);
                //Token = token.ToString();
                var user =await _userManger.FindByEmailAsync(model.Email.ToUpper());//await _userManger.FindByEmailAsync(model.Email).Id;
                return  new { status = "Success", token, user };
            }

            else
            {
                return BadRequest(new { status = "Fail" });

            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task< IActionResult> UpdateUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context._Users.SingleOrDefault(us => us.Id == model.ID);
                if (user == null) return BadRequest(new { status = "Error" });


                user.Email = model.Email;
                user.UserName = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Date_of_Birth = model.Date_of_Birth;
                
                
                //==========================================================================
                var res=await _userManger.UpdateAsync(user);
                //==========================================================================

                //context.SaveChanges();
                //var x= Request.Headers["Authorization"][0].Substring(7);
                //var v = await _userManger.ResetPasswordAsync(user, x, model.Password);
                if(res==IdentityResult.Success)
                    return Ok(new { status="Success",user });
                return BadRequest(new { status = "Fail" });

            }
            return BadRequest(new { status = "Error" });


        }
        [Authorize]
        [HttpPost("change_password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            var user = context._Users.SingleOrDefault(us => us.Id == model.ID);
            if (user == null)
            {
                return BadRequest(new { status = "Error" });
            }
            string code = await _userManger.GeneratePasswordResetTokenAsync(user);
            //var x = Request.Headers["Authorization"][0];//.Substring(7);
            var v = await _userManger.ResetPasswordAsync(user, code, model.Password);
            if (v == IdentityResult.Success)
                return Ok(new { status = "Success" });
            return BadRequest(new { status = "Fail" });

        }


        
        
        private object GenerateJwtToken(string email, User user)
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
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet("login-facebook")]
        public async Task<IActionResult> Signin()
        {
			var allSchemeProvider = (await authenticationSchemeProvider.GetAllSchemesAsync())
                .Select(n => n.DisplayName).Where(n => !String.IsNullOrEmpty(n));

            var res= Challenge(new AuthenticationProperties { RedirectUri = "/" }, allSchemeProvider.FirstOrDefault());

            return Ok();

            //return View(allSchemeProvider);
        }

    [Route("signin/{provider:alpha}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {
            
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, provider);
        }
        
        [Authorize]
        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            var v = User.Identity.IsAuthenticated;
            await HttpContext.SignOutAsync();
            var x = User.Identity.IsAuthenticated;
            //await _signInManager.SignOutAsync();
            return Ok("User loged out successfully");
        }

        [HttpGet("user-registerations")]
        public IActionResult GetUsetReservations(string userId)
        {
            var user = context._Users.Include("Reservations").SingleOrDefault(us => us.Id == userId);
            if (user == null)
                return BadRequest(new { status = "Error" });
            return Ok(new { status = "Success", user });
        }

        /*
         * [HttpPost]
        [Route("account/login/facebook")]
        public async Task<IActionResult> FacebookLoginAsync([FromBody] FacebookLoginResource resource)
        {
            var authorizationTokens = await _accountService.FacebookLoginAsync(resource);
            return Ok(authorizationTokens);
        }*/


    }
}