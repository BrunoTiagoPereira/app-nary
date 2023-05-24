using AppNary.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppNary.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; private set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}