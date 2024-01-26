using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Api.Configuration;
using System.Security.Claims;

namespace NIHR.StudyManagement.Api.Middleware.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void ConfigureForLocalDevelopment(this JwtBearerEvents jwtBearerEvents,
            StudyManagementApiConfiguration studyManagementApiConfiguration)
        {
            if (studyManagementApiConfiguration == null)
            {
                return;
            }

            jwtBearerEvents = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    var claims = new[]
                        {
                        new Claim(ClaimTypes.NameIdentifier,"LocalHostUser",ClaimValueTypes.String,context.Options.ClaimsIssuer),
                        new Claim(ClaimTypes.Name,"LocalHostUser",ClaimValueTypes.String,context.Options.ClaimsIssuer)
                    };

                    context.Principal = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, context.Scheme.Name));

                    context.Token = "ABC";

                    context.Request.Headers["Authorization"] = "Bearer xxx";

                    context.Success();

                    return Task.CompletedTask;
                },

                OnChallenge = context =>
                {
                    //context.HandleResponse();
                    return Task.CompletedTask;
                },

                OnForbidden = context =>
                {
                    //context.Success();
                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    //context.Success();
                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    //context.Success();
                    System.Diagnostics.Debug.Print($"OnAuthenticationFailed Exception: {context.Exception?.Message}");
                    System.Diagnostics.Debug.Print($"OnAuthenticationFailed Failure: {context.Result?.Failure?.Message}");

                    return Task.CompletedTask;
                }
            };
            //    {
            //        if(jwtBearerEvents == null)
            //        {
            //            jwtBearerEvents = new JwtBearerEvents();
            //        }

            //        jwtBearerEvents.OnMessageReceived = context =>
            //        {
            //            var claims = new[]
            //                {
            //              new Claim(
            //                  ClaimTypes.NameIdentifier,
            //                  "LocalHostUser",
            //                  ClaimValueTypes.String,
            //                  context.Options.ClaimsIssuer),
            //              new Claim(
            //                  ClaimTypes.Name,
            //                  "LocalHostUser",
            //                  ClaimValueTypes.String,
            //                  context.Options.ClaimsIssuer)
            //            };

            //            context.Principal = new ClaimsPrincipal(
            //                new ClaimsIdentity(claims, context.Scheme.Name));

            //            context.Success();

            //            return Task.CompletedTask;
            //        };

            //        jwtBearerEvents.OnForbidden = context => {
            //            return Task.CompletedTask;
            //        };

            //        jwtBearerEvents.OnTokenValidated = context => {
            //            return Task.CompletedTask;
            //        };

            //        jwtBearerEvents.OnAuthenticationFailed = context => {

            //            System.Diagnostics.Debug.Print($"OnAuthenticationFailed Exception: {context.Exception?.Message}");
            //            System.Diagnostics.Debug.Print($"OnAuthenticationFailed Failure: {context.Result?.Failure?.Message}");

            //            return Task.CompletedTask;
            //        };
            //    }
        }
    }
}
