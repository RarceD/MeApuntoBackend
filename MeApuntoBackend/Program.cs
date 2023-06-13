
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace MeApuntoBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(o => o.AddPolicy("NUXT", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Repositories;
            LoadRepositories(builder);

            // Services:
            LoadServices(builder);

            // Stats:
            builder.Services.AddSingleton<IStatsService, StatsService>();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            // Background services:
            builder.Services.AddHostedService<PeriodicTaskService>();


            // Add logging
            builder.Services.AddLogging(logging =>
            {
                logging.AddFile("Logs/Log.{Date}.txt", LogLevel.Warning);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("NUXT");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void LoadServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IClientManagementService, ClientManagementService>();
            builder.Services.AddScoped<ICourtManagementService, CourtManagementService>();
            builder.Services.AddScoped<IBookerManagementService, BookerManagementService>();
            builder.Services.AddScoped<IMailService, MailService>();
        }

        private static void LoadRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IClientRepository, ClientsRepository>();
            builder.Services.AddScoped<IUrbaRepository, UrbaRepository>();
            builder.Services.AddScoped<INormativeRepository, NormativeRepository>();
            builder.Services.AddScoped<ICourtRepository, CourtRepository>();
            builder.Services.AddScoped<ISchedulerRepository, SchedulerRepository>();
            builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        }
    }
}