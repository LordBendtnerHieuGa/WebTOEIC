/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.WebToeic.Data.EFCore
{
    public class WebToeicDbContextFactory : IDesignTimeDbContextFactory<WebToeicDbContext>
    {
        public WebToeicDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("WebToeicDb");

            var optionsBuilder = new DbContextOptionsBuilder<WebToeicDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WebToeicDbContext(optionsBuilder.Options);

        }
    }
}
*/