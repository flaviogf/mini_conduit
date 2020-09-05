using System.Linq;
using Conduit.Api.Infrastructure;
using Conduit.Api.Repositories;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IHash, Bcrypt>();

            services.AddScoped<IAuth, Jwt>();

            services.AddScoped<IUserRepository, EFUserRepository>();

            services
                .AddControllers()
                .ConfigureApiBehaviorOptions(it =>
                {
                    it.InvalidModelStateResponseFactory = (context) =>
                    {
                        string[] message = context.ModelState.Select(it => it.Value).SelectMany(it => it.Errors).Select(it => it.ErrorMessage).ToArray();

                        var result = new ObjectResult(new ErrorResponse(message))
                        {
                            StatusCode = 402
                        };

                        return result;
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
