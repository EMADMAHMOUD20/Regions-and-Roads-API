using App.Core.Domain.IdentityModels;
using App.Core.Domain.RepositoryContracts;
using App.Core.Services;
using App.Core.ServicesContract;
using App.Infrastructure.Db;
using App.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;


namespace SimpleProjectWebAPIwithDIandEF.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
           IConfiguration configuration)
        {
           
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            services.AddDbContext<RoadCityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                HttpLoggingFields.ResponsePropertiesAndHeaders; // Log everything (headers, body, etc.)
            });

            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegionRepositoryContract, RegionRepository>();
            services.AddScoped<IRoadService, RoadService>();
            services.AddScoped<IRoadRepositoryContract, RoadRepository>();
            services.AddTransient<IEmailService, EmailService>();

            // enable cors , but rather than * you must but the domain you trusted
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(b =>
                {
                    b.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>());

                });
            });

            // add identity as a service 
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<RoadCityDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, RoadCityDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, RoadCityDbContext, Guid>>();
                
            services.AddTransient<IJwtService, JwtService>();

            //to enable addauthentication middleware to check if JWT submitted or not or check any validation on it
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // challenge execute when jwt failed
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                    option.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(new
                            {
                                statusCode = 401,
                                success = false,
                                message = "Unauthorized - Token is missing or expired, if expired generate new token via [Create-New-Jwt-BaseOn-Refresh-Token]",
                            });
                            await context.Response.WriteAsync(result);
                        }
                    };
                });
            services.AddAuthorization();
            //services.AddAutoMapper(typeof(AutoMapperProfile));
            return services;
        }
    }
}
