namespace WebAPI.Configuration
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowClientApps", policy =>
                {
                    if (env.IsDevelopment())
                    {
                        policy.SetIsOriginAllowed(_ => true); // Allow everything
                    }
                    else
                    {
                        policy.WithOrigins("https://arreglaya.com"); // Prod origins only
                    }

                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}
