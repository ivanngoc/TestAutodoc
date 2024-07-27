using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Context = DAL.EFCore.TasksDbContext;

namespace DAL.EFCore
{
    public class DataContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().Build();

            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new Context(optionsBuilder.Options);
        }
    }
}
