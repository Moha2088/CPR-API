using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CVR_API.Data;
using CVR_API.Repository;
using CVR_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace CVR_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<CVR_APIContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CVR_APIContext") ?? throw new InvalidOperationException("Connection string 'CVR_APIContext' not found.")));

        // Add services to the container.
        builder.Services.AddScoped<IRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "CPR-API",
                Description = "Data Transfer Object exercise",
                Contact = new OpenApiContact
                {
                    Name = "Mohamed Shil",
                    Url = new Uri("https://Github.com/Moha2088")
                }
            });
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("x-version")
                );
        });


        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]
            ?? throw new ArgumentException("JWT Key not found"))),
            };
        });

        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", options =>
            {
                options.Window = TimeSpan.FromSeconds(10);
                options.PermitLimit = 2;
                options.QueueLimit = 1;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            }).RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        builder.Services.AddCors(p => p.AddPolicy("MyPolicy", config =>
        {
            config.AllowAnyHeader();
            config.AllowAnyMethod();
            config.AllowAnyOrigin();
        }));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("MyPolicy");

        app.MapControllers();

        app.Run();
    }
}
