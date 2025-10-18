using BritInsurance.Api.Extensions;
using BritInsurance.Api.Middleware;
using BritInsurance.Application.Mappings;
using BritInsurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomModelValidators();
builder.Services.AddSwagger();
builder.Services.AddApiVersionings();
builder.Services.AddDomainService();
builder.Services.AddApplicationService();
builder.AddApplicationConfigurations();
builder.AddApplicationAuth();

// Log
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// dotnet ef dbcontext scaffold "Server=localhost;Database=BritInsurance;Trusted_Connection=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer --project "src\BritInsurance.Domain" --startup-project "src\BritInsurance.Api" -o Entities --context BritInsuranceDbContext
builder.Services.AddDbContext<BritInsuranceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BritInsuranceDatabase"));
});

builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "BritInsurance API V1");
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();  

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer-when-downgrade";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), camera=()";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    await next();
});

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();