using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Conduit.Presentation.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

                var configuration = CreateConfiguration();

                var evolve = CreateEvolve(configuration);

                evolve.Migrate();

                var host = CreateHost(configuration);

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddJsonFile($"appsettings.Development.json").AddEnvironmentVariables().Build();
        }

        public static Evolve.Evolve CreateEvolve(IConfiguration configuration)
        {
            var location = configuration.GetValue<string>("Evolve:Location");

            var connection = new SqlConnection(configuration.GetConnectionString("Conduit"));

            var evolve = new Evolve.Evolve(connection, it => Log.Information(it))
            {
                Locations = new[] { location },
            };

            return evolve;
        }

        public static IHost CreateHost(IConfiguration configuration)
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(it => it.UseConfiguration(configuration).UseStartup<Startup>()).Build();
        }
    }
}
