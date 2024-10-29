using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Create a configuration object to read appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Presentation/Presentation.Server")) // Adjust path if necessary
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve the connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("SQLiteConnection");

            // Set up DbContext options with the connection string
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite(connectionString); // or UseSqlServer for SQL Server

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
