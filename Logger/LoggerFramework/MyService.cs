using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging;
using Serilog;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoggerFramework
{
    public class MyService : IMyService
    {
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly ILogger<MyService> _logger;

        public MyService(ILogger<MyService> logger)
        {
            //var baseUrl = config[SomeConfigItem:BaseUrl];
            //var token = config[SomeConfigItem:Token];

            //_baseUrl = baseUrl;
            //_token = token;
            // _logger = loggerFactory.CreateLogger<MyService>();
            _logger = logger;
        }

        public async Task MyServiceMethod()
        {
            _logger.LogInformation("Info log from MyService");
        }
    }
}
