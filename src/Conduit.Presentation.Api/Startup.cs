using System;
using System.Data;
using System.Text;
using Conduit.Application;
using Conduit.Domain.Articles;
using Conduit.Domain.Tags;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Articles;
using Conduit.Infrastructure.Tags;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            var connectionString = _configuration.GetConnectionString("Conduit");

            services.AddScoped<IDbConnection>(it => new SqlConnection(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IArticleRepository, DapperArticleRepository>();

            services.AddScoped<ITagRepository, DapperTagRepository>();

            services.AddScoped<IAuth, LocalAuth>();

            services.AddMediatR(AppDomain.CurrentDomain.Load("Conduit.Application"));

            services.AddAuthentication(it =>
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

                var secret = _configuration.GetValue<string>("JwtConfig:Secret");

                var key = Encoding.UTF8.GetBytes(secret);

                it.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
            });

            services.AddSwaggerGen(it =>
            {
                it.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Conduit",
                });

                it.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                it.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddControllers();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(it => it.MapControllers());
        }
    }
}
