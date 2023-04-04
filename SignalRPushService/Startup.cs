using API.Configurations;
using ExternalApi.Api;
using ExternalApi.ConfigurationModel;
using ExternalApi.Interfaces;
using ExternalApi.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SignalRPushService.Models;
using System.Text;

namespace API
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("AppSetting/appsettings.json", true, true)
                .AddJsonFile($"AppSetting/appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Authentication DI
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Keys")["Secret"]);
            services.AddScoped<ExternalApi.TokenService.ISwtToken, ExternalApi.TokenService.SwtToken>();
            services.Configure<SecretKeyModel>(Configuration.GetSection("Keys"));
            services.Configure<ExretnalApiModel>(Configuration.GetSection("ExternalApi"));
            services.AddScoped<ICustomerApi, CustomerApi>();
            services.AddScoped<IPanelUserApi, PanelUserApi>();
           

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = false;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });

            // Swagger Config
            services.AddSwaggerConfiguration();

            services.AddSignalR();
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                        builder =>
                        {
                            builder.AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .SetIsOriginAllowed((host) => true)
                                   .AllowCredentials()
                                   ;
                        }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
               
            }

            app.Use(async (context, next) =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Request.Headers.Add("Authorization", accessToken);
                }

                await next(context);
            });

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalRPushService.Hubs.MainHub>("/signalr");
                endpoints.MapControllers();
            });

            app.UseSwaggerSetup();
        }
    }
}
