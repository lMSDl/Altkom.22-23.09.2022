using DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL
{
    public abstract class Context : DbContext
    {
        protected Context()
        {
        }

        protected Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);

            //modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);

        }

        //public DbSet<Order> Orders { get; set; }

        public bool RandomFail { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (RandomFail)
            {
                if (new Random().Next(1, 25) == 1)
                    throw new Exception();
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}