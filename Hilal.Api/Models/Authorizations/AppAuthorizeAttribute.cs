using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hilal.Common;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;
using Hilal.Service.Interface.v1;
using Hilal.Service.Interface.v1.App;

namespace Hilal.Api.Models.Authorizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AppAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private bool isLoginRequired = true;
        public AppAuthorizeAttribute(bool isLoginRequired)
        {
            this.isLoginRequired = isLoginRequired;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AuthToken tokenData = null;
            string token = string.Empty;
            int language = 0;
            language = (context.HttpContext.Request.Headers.Any(x => x.Key == "language")) ? Convert.ToInt32(context.HttpContext.Request.Headers.Where(x => x.Key == "language").FirstOrDefault().Value.SingleOrDefault()) == (int)ELanguage.English ? (int)ELanguage.English : (int)ELanguage.Arabic : (int)ELanguage.English;
            token = (context.HttpContext.Request.Headers.Any(x => x.Key == "Authorization")) ? context.HttpContext.Request.Headers.Where(x => x.Key == "Authorization").FirstOrDefault().Value.SingleOrDefault().Replace("Bearer ", "") : "";
            if (string.IsNullOrEmpty(token) && isLoginRequired)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(new Response<bool> { IsError = true, Message = Error.MissingAuthorization, Data = false });
                return;
            }

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
                    var keyByteArray = Encoding.ASCII.GetBytes(configuration.GetValue<String>("Tokens:Key"));
                    var signinKey = new SymmetricSecurityKey(keyByteArray);

                    SecurityToken validatedToken;
                    var handeler = new JwtSecurityTokenHandler();
                    var we = handeler.ValidateToken(token, new TokenValidationParameters
                    {
                        IssuerSigningKey = signinKey,
                        ValidAudience = "Audience",
                        ValidIssuer = "Issuer",
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(0)
                    }, out validatedToken);

                    var temp = handeler.ReadJwtToken(token);
                    tokenData = JsonConvert.DeserializeObject<AuthToken>(temp.Claims.FirstOrDefault(x => x.Type.Equals("token"))?.Value);
                    context.RouteData.Values.Add("userId", tokenData.UserId);
                    context.RouteData.Values.Add("DeviceToken", tokenData.DeviceToken);
                    context.RouteData.Values.Add("Language", language);
                }
                catch (Exception ex)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new Response<bool> { IsError = true, Message = Error.AccessDenied, Data = false });
                    return;
                }
                var accountService = (IAppAccountService)context.HttpContext.RequestServices.GetService(typeof(IAppAccountService));
                var isAuthenticated = accountService.IsTokenValid(tokenData.UserId, tokenData.DeviceToken);

                if (!isAuthenticated)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new Response<bool> { IsError = true, Message = Error.AccessDenied, Data = false });
                    return;
                }

                var isAccountVerify = accountService.IsAccountVerified(tokenData.UserId);
                if (!isAccountVerify)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new Response<bool> { IsError = true, Message = Error.AccoutNotVerified, Data = false });
                    return;
                }
            }
            else
            {
                context.RouteData.Values.Add("userId", Guid.Empty.ToString());
                context.RouteData.Values.Add("DeviceToken", "");
                context.RouteData.Values.Add("Language", language);
            }
        }
    }
}
