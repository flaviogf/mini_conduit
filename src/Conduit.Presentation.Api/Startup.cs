using System;
using System.Data;
using System.Linq;
using System.Text;
using Conduit.Application;
using Conduit.Domain.Users;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            services.AddScoped<IDbConnection>(it => new SqlConnection(_configuration.GetConnectionString("Conduit")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IHash, BcryptHash>();

            services.AddScoped<IToken, JwtToken>();

            services.AddScoped<IUserRepository, DapperUserRepository>();

            services.AddMediatR(AppDomain.CurrentDomain.Load("Conduit.Application"));

            services
                .AddAuthentication(it =>
                {
                    it.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    it.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(it =>
                {
                    it.RequireHttpsMetadata = false;

                    it.SaveToken = true;

                    it.TokenValidationParameters.ValidateAudience = false;
                    it.TokenValidationParameters.ValidateIssuer = false;
                    it.TokenValidationParameters.ValidateIssuerSigningKey = true;

                    var secret = _configuration.GetValue<string>("Jwt:Secret");

                    var key = Encoding.UTF8.GetBytes(secret);

                    it.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
                });

            services
                .AddControllers()
                .AddNewtonsoftJson()
                .ConfigureApiBehaviorOptions(it =>
                {
                    it.InvalidModelStateResponseFactory = (context) =>
                    {
                        var errors = context.ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage);

                        return new UnprocessableEntityObjectResult(Envelope.Failure(errors));
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conduit", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(it =>
            {
                it.SwaggerEndpoint("/swagger/v1/swagger.json", "Conduit");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(it => it.MapControllers());
        }
    }
}
