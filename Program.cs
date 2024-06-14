using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CVR_API.Data;
using CVR_API.Repository;
using CVR_API.Services;

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
        builder.Services.AddSwaggerGen();



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
