using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

new WebHostBuilder()
    .UseKestrel()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddJsonFile($"gateway.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        var authenticationProviderKey = "Bearer";
        Action<JwtBearerOptions> option = o =>
        {
            o.Authority = builder.Configuration["IdentityServer:Authority"];
            o.RequireHttpsMetadata = false;
            o.Audience = builder.Configuration["IdentityServer:Audience"];
        };
        services.AddAuthentication().AddJwtBearer(authenticationProviderKey, option);
        services.AddOcelot();
        services.AddHealthChecks();
        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
    })
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        logging.AddConsole();
        logging.AddDebug();
        logging.AddEventSourceLogger();
    })
    .Configure(app =>
    {
        app.UseRouting();
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapHealthChecks("/healthz");

        });
        app.UseCors(MyAllowSpecificOrigins);
        app.UseWebSockets();
        app.UseOcelot().Wait();
    }
    ).Build().Run();
