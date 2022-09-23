using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL.SqlServer
{
    public class SqlServerContext : Context
    {
        public SqlServerContext()
        {
        }

        public SqlServerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer();

            //optionsBuilder.UseChangeTrackingProxies();

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(x => x.Price).HasDefaultValue(0.5);
            modelBuilder.Entity<Order>().Property(x => x.Created).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Order>().Property(x => x.Updated).HasDefaultValueSql("getdate()");


            modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties())
                .Where(x => x.PropertyInfo?.PropertyType == typeof(DateTime)).ToList()
                .ForEach(x =>
                {
                    x.SetColumnType("datetime");
                    x.SetColumnOrder(1);
                });
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }


        public override int SaveChanges()
        {
            ChangeTracker.Entries<IUpdated>()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ToList()
                .ForEach(x => x.Updated = DateTime.Now);

            return base.SaveChanges();
        }
    }
}