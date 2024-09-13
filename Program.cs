using Microsoft.EntityFrameworkCore;
using LemonMES.Database;
using Azure.Identity;
using Microsoft.Data.SqlClient;



namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");

            if (string.IsNullOrEmpty(connection))
            {
                // Fallback to environment variable if not found in configuration
                connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
            }

            if (string.IsNullOrEmpty(connection))
            {
                throw new InvalidOperationException("Connection string 'AZURE_SQL_CONNECTIONSTRING' not found.");
            }

            builder.Services.AddDbContext<PlantDbContext>(options =>
                options.UseSqlServer(connection));

            builder.Services.AddScoped<PlantOperations>();
            builder.Services.AddScoped<PlantStatistics>();
            builder.Services.AddScoped<LogInFunctions>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            // Add logging
            builder.Logging.AddConsole();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession(); 
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();
       
            app.Run();
        }
    }
}
