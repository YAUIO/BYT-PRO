using BYTPRO.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BYTPRO.Test;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDataServices();
    }
}