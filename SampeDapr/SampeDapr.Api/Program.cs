using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using SampeDapr.Application;
using SampeDapr.Application.Services;
using SampeDapr.Application.Shared;
using SampeDapr.OutService;
using SampeDapr.Persistence.MySql;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "sample-dapr-api",
            builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
});

builder.Services.ConfigureApplicationSharedServices(builder.Configuration);
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigurePersistenceService(builder.Configuration);
builder.Services.ConfigOutService(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}).AddDapr();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//Config upload file 
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
{
    op.SwaggerDoc("v1", new OpenApiInfo { Title = "sample-dapr-api", Version = "1.0" });
    op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please input valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",

    });
    op.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             new string[] {}
        }
    });
});
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/healthz");
app.UseCors("sample-dapr-api");
app.UseRouting();
app.MapControllers();
app.UseAuthorization();
app.UseCloudEvents();
app.MapSubscribeHandler();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalStatusHubClient>("/SingalStatus");
});
app.UseStaticFiles();
app.Run();
