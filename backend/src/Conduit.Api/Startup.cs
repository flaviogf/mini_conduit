using System.Linq;
using System.Reflection;
using AutoMapper;
using Conduit.Api.Repositories;
using Conduit.Api.ViewModels;
using Conduit.Core.Articles;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conduit.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IArticleRepository, EFArticleRepository>();

            services.AddMediatR(Assembly.GetAssembly(typeof(Article)));

            services.AddAutoMapper(it =>
            {
                it.CreateMap<CreateArticleViewModel, CreateArticleRequest>();
                it.CreateMap<CreateArticleResponse, ArticleViewModel>();
            }, Assembly.GetExecutingAssembly());

            services
                .AddControllers()
                .ConfigureApiBehaviorOptions(it =>
                {
                    it.InvalidModelStateResponseFactory = (context) =>
                     {
                         string[] errors = context.ModelState.Select(it => it.Value).SelectMany(it => it.Errors).Select(it => it.ErrorMessage).ToArray();

                         var response = new ErrorResponseViewModel(errors);

                         var result = new ObjectResult(response)
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

            app.UseEndpoints(it => it.MapControllers());
        }
    }
}
