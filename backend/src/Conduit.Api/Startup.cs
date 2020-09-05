using System.Linq;
using System.Text;
using Conduit.Api.Infrastructure;
using Conduit.Api.Repositories;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(it => it.UseSqlite(_configuration.GetConnectionString("ApplicationDbContext")));

            services.AddHttpContextAccessor();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IHash, Bcrypt>();

            services.AddScoped<IAuth, Jwt>();

            services.AddScoped<IUserRepository, EFUserRepository>();

            services.AddScoped<IArticleRepository, EFArticleRepository>();

            services
                .AddControllers()
                .AddNewtonsoftJson()
                .ConfigureApiBehaviorOptions(it =>
                {
                    it.InvalidModelStateResponseFactory = (context) =>
                    {
                        string[] message = context.ModelState.Select(it => it.Value).SelectMany(it => it.Errors).Select(it => it.ErrorMessage).ToArray();

                        var result = new ObjectResult(new ErrorResponse(message))
                        {
                            StatusCode = 422
                        };

                        return result;
                    };
                });

            services
                .AddAuthentication(it =>
                {
                    it.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    it.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(it =>
                {
                    byte[] key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Secret"));

                    it.RequireHttpsMetadata = false;

                    it.SaveToken = true;

                    it.TokenValidationParameters.ValidateIssuer = false;
                    it.TokenValidationParameters.ValidateAudience = false;
                    it.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    it.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
