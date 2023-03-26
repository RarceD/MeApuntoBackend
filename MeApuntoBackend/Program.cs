
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.EntityFrameworkCore;

namespace MeApuntoBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Repositories;
            LoadRepositories(builder);

            // Services:
            LoadServices(builder);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void LoadServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IClientManagementService, ClientManagementService>();
        }

        private static void LoadRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IClientRepository, ClientsRepository>();
            builder.Services.AddScoped<IBookerRepository, BookerRepository>();
            builder.Services.AddScoped<IUrbaRepository, UrbaRepository>();
        }
    }
}