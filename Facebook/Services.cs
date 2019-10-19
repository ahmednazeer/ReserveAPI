/*
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySQLIdentity.Models;
using Newtonsoft.Json;
using Serenity.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MySQLIdentity.Facebook
{
    public class AccountService
    {
        private readonly FacebookService _facebookService;
        private readonly JwtHandler _jwtHandler;

        public AccountService(FacebookService facebookService, JwtHandler jwtHandler)
        {
            _facebookService = facebookService;
            _jwtHandler = jwtHandler;
        }

        public async Task<AuthorizationTokensResource> FacebookLoginAsync(FacebookLoginResource facebookLoginResource)
        {
            if (string.IsNullOrEmpty(facebookLoginResource.facebookToken))
            {
                throw new Exception("Token is null or empty");
            }

            var facebookUser = await _facebookService.GetUserFromFacebookAsync(facebookLoginResource.facebookToken);
            var domainUser = await UnitOfWork.Users.GetAsync(facebookUser.Email);

            return await CreateAccessTokens(domainUser, facebookLoginResource.DeviceId,
                facebookLoginResource.DeviceName);
        }

        private async Task<AuthorizationTokensResource> CreateAccessTokens(User user, string deviceId,
            string deviceName)
        {
            var accessToken = _jwtHandler.CreateAccessToken(user.Id, user.Email);
            var refreshToken = _jwtHandler.CreateRefreshToken(user.Id);

            return new AuthorizationTokensResource { AccessToken = accessToken, RefreshToken = refreshToken };
        }
    }

    public class FacebookService
    {
        private readonly HttpClient _httpClient;

        public FacebookService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<User> GetUserFromFacebookAsync(string facebookToken)
        {
            var result = await GetAsync<dynamic>(facebookToken, "me", "fields=first_name,last_name,email,picture.width(100).height(100)");
            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }

            var account = new User()
            {
                Email = result.email,
                FirstName = result.first_name,
                LastName = result.last_name,
                //Picture = result.picture.data.url
            };

            return account;
        }

        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }






    public class JwtHandler
    {
        private readonly IConfiguration _configuration;

        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResource CreateAccessToken(Guid userId, string email)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            //new Claim(ClaimTypes.Role, role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //new Claim(JwtRegisteredClaimNames.Iat, now.ToTimeStamp().ToString(), ClaimValueTypes.Integer64),
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(double.Parse(_configuration["Tokens:AccessExpireMinutes"]));
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, expiry.ToTimeStamp());
        }

        public TokenResource CreateRefreshToken(Guid userId)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToTimeStamp().ToString(), ClaimValueTypes.Integer64),
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(double.Parse(_configuration["Tokens:RefreshExpireMinutes"]));
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, expiry.ToTimeStamp());
        }

        private JwtSecurityToken CreateSecurityToken(IEnumerable<Claim> claims, DateTime now, DateTime expiry, SigningCredentials credentials)
            => new JwtSecurityToken(claims: claims, notBefore: now, expires: expiry, signingCredentials: credentials);

        private static TokenResource CreateTokenResource(string token, long expiry)
            => new TokenResource { Token = token, Expiry = expiry };
    }





}
*/
