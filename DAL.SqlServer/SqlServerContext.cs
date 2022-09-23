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
                optionsBuilder.UseSqlServer(x => x.UseNetTopologySuite());

            //optionsBuilder.UseChangeTrackingProxies();

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(x => x.Price)//.HasDefaultValue(0.5);
                                                                .HasDefaultValueSql("NEXT VALUE FOR sequences.ProductPrice");
            //modelBuilder.Entity<Order>().Property(x => x.Created).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Order>().Property<DateTime>("Created").HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Order>().Property<string>("Metadata");
            modelBuilder.Entity<Order>().Property(x => x.Updated).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Product>().Property(x => x.Description).HasComputedColumnSql("[Name] + ' ' + STR([Price]) + 'zł'", stored: true);

            // modelBuilder.Entity<OrderSummary>().ToTable(name: null);
            modelBuilder.Entity<OrderSummary>().ToView("View_OrderSummary");

            modelBuilder.Entity<Order>().Property<int>("DeletedProducts");

            modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties())
                .Where(x => x.PropertyInfo?.PropertyType == typeof(DateTime)).ToList()
                .ForEach(x =>
                {
                    x.SetColumnType("datetime");
                    x.SetColumnOrder(1);
                });

            modelBuilder.HasSequence<int>("ProductPrice", "sequences")
                .StartsAt(100)
                .HasMax(300)
                .HasMin(30)
                .IsCyclic()
                .IncrementsBy(33);




            modelBuilder.Entity<ProductDetails>().ToTable("Product");
            modelBuilder.Entity<Product>().HasOne(x => x.ProductDetails).WithOne().HasForeignKey<ProductDetails>(x => x.Id);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            //configurationBuilder.Properties<OrderTypes>().HaveConversion<string>();

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