using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NIHR.StudyManagement.Api.Configuration;
using NIHR.StudyManagement.Api.Middleware.Extensions;
using System.Security.Claims;

namespace NIHR.StudyManagement.Api;
public class AuthenticationConfig
{
    public bool overrideJwtValidation { get; set; }
}
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        var studyManagementApiConfigurationSection = Configuration.GetSection("StudyManagementApiConfiguration");

        var studyManagementApiConfiguration = studyManagementApiConfigurationSection.Get<StudyManagementApiConfiguration>() ?? throw new ArgumentNullException(nameof(StudyManagementApiConfiguration));

        services.Configure<StudyManagementApiConfiguration>(studyManagementApiConfigurationSection);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddJwtBearer(options =>
        {
            options.Authority = studyManagementApiConfiguration.JwtTokenValidationConfiguration.Authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false
            };

            //if (studyManagementApiConfiguration != null
            //    && studyManagementApiConfiguration.JwtTokenValidationConfiguration.OverrideJwtTokenValidation)
            //{
            //    options.Events.ConfigureForLocalDevelopment(studyManagementApiConfiguration);
            //}

            //options.Events.ConfigureForLocalDevelopment();

            if(studyManagementApiConfiguration.JwtTokenValidationConfiguration.OverrideJwtTokenValidation)
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var claims = new List<Claim>();

                        foreach (var claimConfig in studyManagementApiConfiguration.JwtTokenValidationConfiguration.ClaimsConfigurations)
                        {
                            claims.Add(new Claim(claimConfig.Name, claimConfig.Description));
                        }

                        context.Principal = new ClaimsPrincipal(
                            new ClaimsIdentity(claims, context.Scheme.Name));

                        context.Success();

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                    },

                    OnForbidden = context =>
                    {
                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            }

            if (1 == 2)
            {
                options.Events = new JwtBearerEvents()
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
            }

        });

        services.AddAuthorization(options =>
        {
            //options.AddPolicy("Admin", policy => policy.RequireClaim("cognito:groups", "AdminGroup"));

            //options.AddPolicy(PolicyNames.StudyManagement_Api_Study_Create, policy => policy.Requirements.Add(new HasScopeRequirement(ScopeNames.StudyManagement_Api_Study_Create)));
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //endpoints.MapGet("/", async context =>
            //{
            //    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            //});
        });
    }

    private static JwtBearerEvents ConfigureForLocalDevelopment(JwtBearerEvents jwtBearerEvents)
    {
        jwtBearerEvents.OnMessageReceived = context =>
        {
            var claims = new[]
                {
                  new Claim(
                      ClaimTypes.NameIdentifier,
                      "LocalHostUser",
                      ClaimValueTypes.String,
                      context.Options.ClaimsIssuer),
                  new Claim(
                      ClaimTypes.Name,
                      "LocalHostUser",
                      ClaimValueTypes.String,
                      context.Options.ClaimsIssuer)
                };

            context.Principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, context.Scheme.Name));

            context.Success();

            return Task.CompletedTask;
        };

        return jwtBearerEvents;
    }

    private static void dosomething()
    {
        /*
         *                 var claims = new[]
                {
                  new Claim(
                      ClaimTypes.NameIdentifier,
                      "LocalHostUser",
                      ClaimValueTypes.String,
                      context.Options.ClaimsIssuer),
                  new Claim(
                      ClaimTypes.Name,
                      "LocalHostUser",
                      ClaimValueTypes.String,
                      context.Options.ClaimsIssuer)
                };

                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();
              }
              return Task.CompletedTask;
         * */
    }
}