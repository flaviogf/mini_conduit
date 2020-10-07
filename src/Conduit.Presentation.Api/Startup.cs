using System;
using System.Data;
using Conduit.Domain.Tags;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Tags;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Conduit.Presentation.Api
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
            var connectionString = _configuration.GetConnectionString("Conduit");

            services.AddScoped<IDbConnection>(it => new SqlConnection(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITagRepository, DapperTagRepository>();

            services.AddMediatR(AppDomain.CurrentDomain.Load("Conduit.Application"));

            services.AddControllers();

            services.AddSwaggerGen(it =>
            {
                it.SwaggerDoc("v1", new OpenApiInfo { Title = "Conduit", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conduit");
            });

            app.UseRouting();

            app.UseEndpoints(it => it.MapControllers());
        }
    }
}
