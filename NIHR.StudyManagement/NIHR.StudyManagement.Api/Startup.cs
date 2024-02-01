using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NIHR.StudyManagement.API.Configuration;
using System.Security.Claims;

namespace NIHR.StudyManagement.API;

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

        var studyManagementApiConfigurationSection = Configuration.GetSection("StudyManagementApi");

        var studyManagementApiSettings = studyManagementApiConfigurationSection.Get<StudyManagementApiSettings>() ?? throw new ArgumentNullException(nameof(StudyManagementApiSettings));

        services.Configure<StudyManagementApiSettings>(studyManagementApiConfigurationSection);

        // DEBUG
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(studyManagementApiSettings));
        // end debug

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddJwtBearer(options =>
        {
            options.Authority = studyManagementApiSettings.JwtBearer.Authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = studyManagementApiSettings.JwtBearer.ValidateIssuerSigningKey,
                ValidateAudience = studyManagementApiSettings.JwtBearer.ValidateAudience
            };

            // If local settings have a configuration value to override jwt token validation, then add
            // some custom handlers to intercept jwt validation events. Note, this bypasses true authentication
            // and should only be used in a local development environment. Claims can be mocked from the same configuration setting
            if(studyManagementApiSettings.JwtBearer.JwtBearerOverrideSettings != null
                && studyManagementApiSettings.JwtBearer.JwtBearerOverrideSettings.OverrideEvents)
            {
                var events = ConfigureForLocalDevelopment(studyManagementApiSettings);

                if(events != null)
                {
                    options.Events = events;
                }
            }
        });

        services.AddAuthorization();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    /// <summary>
    /// This method is to override jwt token validation so that during local development there are no external dependencies, even
    /// for authentication.
    /// Configuration is controlled via appsettings values.
    /// </summary>
    /// <param name="studyManagementApiSettings"></param>
    /// <returns></returns>
    private static JwtBearerEvents? ConfigureForLocalDevelopment(StudyManagementApiSettings studyManagementApiSettings)
    {
        if(studyManagementApiSettings.JwtBearer.JwtBearerOverrideSettings == null
            || !studyManagementApiSettings.JwtBearer.JwtBearerOverrideSettings.OverrideEvents)
        {
            return null;
        }    

        return new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var claims = new List<Claim>();

                foreach (var claimConfig in studyManagementApiSettings.JwtBearer.JwtBearerOverrideSettings.ClaimsOverride)
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
}