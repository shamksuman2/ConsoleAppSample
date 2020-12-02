using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace LoggerFramework
{
    public class Startup1
    {
        IConfigurationRoot Configuration { get; }
        IServiceCollection services;
        public Startup1()
        {
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDirectory = Path.GetFullPath(Path.Combine(binDirectory, ".."));

            var config = new ConfigurationBuilder()
                                        .SetBasePath(rootDirectory)
                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //configure console logging
            ConfigureLogging(services);
            RegisterServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Startup1>();

            logger.LogDebug("Logger is working!");

            var service = serviceProvider.GetService<IMyService>();
            service.MyServiceMethod();
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                Enum.TryParse(Configuration.GetSection("Logging:LogLevel:Default").Value, out LogLevel enumValue);
                //builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>(Fleet App, enumValue);
                //builder.AddApplicationInsights(Configuration.GetSection(ApplicationInsights:InstrumentationKey).Value);
                builder.AddConsole();
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IMyService, MyService>();

        }
    }
}
