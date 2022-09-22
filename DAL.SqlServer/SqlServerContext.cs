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
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer();

            //optionsBuilder.UseChangeTrackingProxies();

            base.OnConfiguring(optionsBuilder);
        }
    }
}