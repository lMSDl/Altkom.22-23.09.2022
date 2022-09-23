using Microsoft.EntityFrameworkCore;

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

    }
}