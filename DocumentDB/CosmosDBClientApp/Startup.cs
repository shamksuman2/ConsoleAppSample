using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace LoggerFramework
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        ILogger _logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
             //.SetBasePath(Directory.GetCurrentDirectory())
             //.AddJsonFile(appsettings.json, optional: true, reloadOnChange: true)
             //.AddEnvironmentVariables()
             .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //configure console logging
            ConfigureLogging(services);
            RegisterServices(services);

           // var serviceProvider = services.BuildServiceProvider();
            //var service = serviceProvider.GetService<IMyService>();
           // service.MyServiceMethod();
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            //services.AddLogging();

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
            //services.AddSingleton<IMyService, MyService>();

        }
    }
}
