using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Auth.Jwt
{
    public static class JwtServiceExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var key = Encoding.UTF8.GetBytes(jwtSettings!.Secret ?? String.Empty);

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddSingleton<JwtTokenGenerator>();

            // Adding JWT Authentication
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // ✅ Look for token in HttpOnly cookie named "AccessToken"
                            if (context.Request.Cookies.TryGetValue("AccessToken", out var token))
                            {
                                context.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
