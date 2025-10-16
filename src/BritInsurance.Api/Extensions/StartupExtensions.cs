using Asp.Versioning;
using BritInsurance.Application.Dto;
using BritInsurance.Application.Interface;
using BritInsurance.Application.Validation;
using BritInsurance.Domain.Interface;
using BritInsurance.Infrastructure.Config;
using BritInsurance.Infrastructure.Data;
using BritInsurance.Infrastructure.Data.Repositories;
using BritInsurance.Infrastructure.Identity;
using BritInsurance.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BritInsurance.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCustomModelValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateItemDto>, CreateItemValidator>();
            services.AddScoped<IValidator<UpdateItemDto>, UpdateItemValidator>();
            services.AddScoped<IValidator<CreateProductDto>, CreateProductValidator>();
            services.AddScoped<IValidator<UpdateProductDto>, UpdateProductValidator>();
            services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
            services.AddScoped<IValidator<RefreshTokenDto>, RefreshTokenDtoValidator>();

            services.AddFluentValidationAutoValidation();

            return services;
        }

        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IItemService, ItemService>();

            return services;
        }

        public static WebApplicationBuilder AddApplicationAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetSection("IdentitySettings:Issuer").Value,
                    ValidAudience = builder.Configuration.GetSection("IdentitySettings:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("IdentitySettings:Key").Value!))
                };
            });
            builder.Services.AddAuthorization();

            return builder;
        }

        public static IServiceCollection AddDomainService(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static WebApplicationBuilder AddApplicationConfigurations(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<IdentityConfig>(builder.Configuration.GetSection("IdentitySettings"));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(m => m.Value.Errors.Count > 0)
                        .SelectMany(m => m.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(new { errors });
                };
            });

            return builder;
        }

        public static IServiceCollection AddApiVersionings(this IServiceCollection services)
        {
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ReportApiVersions = true;
                option.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
                .AddMvc()
                .AddApiExplorer(option =>
                {
                    option.GroupNameFormat = "'v'VVV";
                    option.SubstituteApiVersionInUrl = true;
                    option.AssumeDefaultVersionWhenUnspecified = true;
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "BritInsurance API",
                    Version = "v1",
                    Description = "API for BritInsurance"
                });

                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                option.AddSecurityDefinition("jwt_auth", securityDefinition);

                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    { securityScheme, new string[] { } },
                };
                option.AddSecurityRequirement(securityRequirements);
            });

            return services;
        }
    }
}