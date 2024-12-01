using COPiTOS_IntegrationTests.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;

namespace COPiTOS_IntegrationTests;

public class BaseController
{
    public HttpClient _client { get; }

    public WebApplicationFactory<Program> _factory { get; }
    
    public BaseController()
    {
        _factory = new CustomWebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        _client = _factory.CreateClient();
    }
}