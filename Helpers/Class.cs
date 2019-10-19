using Microsoft.AspNetCore.Authorization;
using MySQLIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySQLIdentity.Helpers
{
    /*
    public class FacebookRequirement : AuthorizationHandler<FacebookRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FacebookRequirement requirement)
        {
            var socialConfig = new SocialConfig { Facebook = new SocialApp { AppId = "<FacebookAppId>", AppSecret = "<FacebookAppSecret>" } };
            var socialservice = new SocialAuthService(socialConfig);

            var authorizationFilterContext = context.Resource as AuthorizationFilterContext;
            if (authorizationFilterContext == null)
            {
                context.Fail();
                return Task.FromResult(0);
            }

            var httpContext = authorizationFilterContext.HttpContext;
            if (httpContext != null && httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var authorizationHeaders = httpContext.Request.Headers.Where(x => x.Key == "Authorization").ToList();
                var token = authorizationHeaders.FirstOrDefault(header => header.Key == "Authorization").Value.ToString().Split(' ')[1];

                var user = socialservice.VerifyTokenAsync(new ExternalToken { Provider = "Facebook", Token = token }).Result;
                if (!user.IsVerified)
                {
                    context.Fail();
                    return Task.FromResult(0);
                }

                context.Succeed(requirement);
                return Task.FromResult(0);
            }

            context.Fail();
            return Task.FromResult(0);
        }
    }

    public class SocialConfig
    {
        public SocialApp Facebook { get; set; }
    }

    public class SocialApp
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }

    public class ExternalToken
    {
        public string Provider { get; set; }
        public string Token { get; set; }
    }


    public class SocialAuthService
    {
        private SocialConfig SocialConfig { get; set; }

        public SocialAuthService(SocialConfig socialConfig)
        {
            SocialConfig = socialConfig;
        }

        public async Task<User> VerifyTokenAsync(ExternalToken exteralToken)
        {
            switch (exteralToken.Provider)
            {
                case "Facebook":
                    return await VerifyFacebookTokenAsync(exteralToken.Token);
                default:
                    return null;
            }
        }

        private async Task<User> VerifyFacebookTokenAsync(string token)
        {
            var user = new User();
            var client = new HttpClient();

            var verifyTokenEndPoint = string.Format("https://graph.facebook.com/me?access_token={0}&fields=email,name", token);
            var verifyAppEndpoint = string.Format("https://graph.facebook.com/app?access_token={0}", token);

            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic userObj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                uri = new Uri(verifyAppEndpoint);
                response = await client.GetAsync(uri);
                content = await response.Content.ReadAsStringAsync();
                dynamic appObj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                if (appObj["id"] == SocialConfig.Facebook.AppId)
                {
                    //token is from our App
                    user.SocialUserId = userObj["id"];
                    user.Email = userObj["email"];
                    user.Name = userObj["name"];
                    user.IsVerified = true;
                }

                return user;
            }
            return user;
        }
    }

    */
}
