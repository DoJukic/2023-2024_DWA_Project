using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using DBScaffold.Models;
using DBScaffold;
using Microsoft.OpenApi.Models;
using SessionLoggerMiddleware;

namespace RESTful_Service_Module
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddTransientSessionLoggerMidAuthPls();

            builder.Services.AddControllers();

            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            if (allowedOrigins == null)
                allowedOrigins = new string[0];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CustomCors",
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "RWA Web API", Version = "v1" });

                option.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter valid JWT",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                option.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new List<string>()
                        }
                    });
            });

            // Configure JWT security services
            var secureKey = builder.Configuration["JWT:SecureKey"];
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => {
                    var Key = Encoding.UTF8.GetBytes(secureKey);
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Key)
                    };
                });

            // WATCH FOR DOUBLE "\"!
            // dotnet ef dbcontext scaffold "name=ConnectionStrings:DB" Microsoft.EntityFrameworkCore.SqlServer -o Models --force
            builder.Services.AddDbContext<DwaContext>(options => {
                options.UseSqlServer("name=ConnectionStrings:DB");
            });

            builder.Services.AddControllersWithViews() // Circular references are dead, long live monitored circular references!
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CustomCors");

            // Use authentication / authorization middleware
            app.UseAuthentication();
            app.UseSessionLoggerMidAuthPls();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
