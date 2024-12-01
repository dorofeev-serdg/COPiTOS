using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace COPiTOS_IntegrationTests.Configuration;

public class CustomWebApplicationFactory<TProgram>: WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => {});
        builder.UseEnvironment("Development");
    }  
}