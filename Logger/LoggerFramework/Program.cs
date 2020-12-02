using Microsoft.Extensions.DependencyInjection;
using System;

namespace LoggerFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();
            startup.ConfigureServices(services);
        }
    }
}
