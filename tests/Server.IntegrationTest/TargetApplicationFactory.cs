using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Server;

namespace Server.IntegrationTest
{
    public class TargetApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("DevelopmentLocal");
            builder.UseSetting("ConnectionStrings:DefaultConnection", Config.DEF_CONN_STRING);

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            });

            builder.ConfigureServices(services => { });
        }
    }
}