using System;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using Conduit.Api.Database;
using Conduit.Api.Infrastructure;
using Conduit.Api.Models;
using Conduit.Api.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            services.AddDbContext<ConduitDbContext>(it => it.UseSqlite(_configuration.GetConnectionString("ConduitDbContext")));

            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ConduitDbContext>();

            services.Configure<IdentityOptions>(it =>
            {
                it.Password.RequireDigit = false;
                it.Password.RequiredLength = 6;
                it.Password.RequiredUniqueChars = 1;
                it.Password.RequireLowercase = false;
                it.Password.RequireNonAlphanumeric = false;
                it.Password.RequireUppercase = false;

                it.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(it =>
            {
                it.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                it.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(it =>
            {
                var secret = _configuration.GetValue<string>("JwtConfig:Secret");

                var key = Encoding.UTF8.GetBytes(secret);

                it.RequireHttpsMetadata = false;

                it.SaveToken = true;

                it.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<ITokenManager<User>, JwtTokenManager<User>>();

            services.AddAutoMapper(it =>
            {
                it.CreateMap<Article, ArticleViewModel>();
                it.CreateMap<ArticleTag, ArticleTagViewModel>();
                it.CreateMap<Tag, TagViewModel>();
                it.CreateMap<User, UserViewModel>();
                it.CreateMap<UserSubscription, UserSubscriptionViewModel>();
            }, Assembly.GetExecutingAssembly());

            services
                .AddControllers()
                .AddNewtonsoftJson(it =>
                {
                    it.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(it =>
                {
                    it.InvalidModelStateResponseFactory = (context) =>
                    {
                        var errors = context.ModelState.SelectMany(it => it.Value.Errors).Select(it => it.ErrorMessage);

                        return new UnprocessableEntityObjectResult(Response.Failure(errors));
                    };
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
