using Microsoft.Azure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace LoggerFramework
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }
        Microsoft.Extensions.Logging.ILogger logger;
        IServiceCollection services;
        public Startup()
        {
           
            Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables()
             .Build();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            Log.Information("Logger is working! from program main");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //configure console logging
            ConfigureLogging(services);
            RegisterServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IMyService>();
            service.MyServiceMethod();
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging();

            //services.AddLogging(
            //    builder =>
            //{
            //    Enum.TryParse(Configuration.GetSection(Logging:LogLevel:Default).Value, out Microsoft.Azure.Storage.LogLevel enumValue);
            //    //builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>(Fleet App, enumValue);
            //    //builder.AddApplicationInsights(Configuration.GetSection(ApplicationInsights:InstrumentationKey).Value);
            //    //builder.AddSerilog(logger);
            //    //builder.AddApplicationInsights();
            //});
        }

        private void RegisterServices(IServiceCollection services)
        {
            //services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IMyService, MyService>();

        }
    }
}
