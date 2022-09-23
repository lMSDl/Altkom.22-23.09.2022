using DAL;
using DAL.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Models;
using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;
using System.Diagnostics;

var contextOptions = new DbContextOptionsBuilder<SqlServerContext>()
    .LogTo(x => Debug.WriteLine(x))
    //LazyLoading
    .UseLazyLoadingProxies()
    .UseSqlServer(@"Server=.\SqlExpress;Database=EF6;Integrated Security=True", x => x.UseNetTopologySuite());

await Transactions(contextOptions);


using (var context = new SqlServerContext(contextOptions.Options))
{
    var orders = context.Set<Order>().ToList();

    for (int i = 0; i < orders.Count; i++)
    {
        orders[i].DeliveryPoint = new Point(52 + i, 21 - i / 10) { SRID = 4326 };
    }


    context.SaveChanges();

    var point = new Point(52, 21) { SRID = 4326 };
    var result = context.Set<Order>().Select(x => x.DeliveryPoint.Distance(point)).ToList();
    var oders = context.Set<Order>().OrderBy(x => x.DeliveryPoint.Distance(point)).ToList();


    var polygon = new Polygon(new LinearRing(new Coordinate[] { new Coordinate(52, 21), new Coordinate(51, 20), new Coordinate(52, 19), new Coordinate(53, 20),  new Coordinate(52, 21) })) { SRID = 4326 };

    orders = context.Set<Order>().Where(x => polygon.Intersects(x.DeliveryPoint)).ToList();
    orders = context.Set<Order>().Where(x => x.DeliveryPoint.IsWithinDistance(point, 150000)).ToList();


}


static void ChangeTrackingAndConcurrencyToken(DbContextOptionsBuilder<SqlServerContext> contextOptions)
{
    var context = new SqlServerContext(contextOptions.Options);

    context.Database.EnsureDeleted();
    //context.Database.EnsureCreated();
    context.Database.Migrate();

    var order = new Order();
    var product = new Product();
    order.Products.Add(product);

    Console.WriteLine(context.Entry(order).State);
    context.Attach(order);
    Console.WriteLine(context.Entry(order).State);
    context.Add(order);
    Console.WriteLine(context.Entry(order).State);
    context.SaveChanges();
    Console.WriteLine(context.Entry(order).State);

    order.DateTime = DateTime.Now;
    Console.WriteLine(context.Entry(order).State);
    context.SaveChanges();
    Console.WriteLine(context.Entry(order).State);

    context.Remove(order);
    Console.WriteLine(context.Entry(order).State);
    context.SaveChanges();
    Console.WriteLine(context.Entry(order).State);

    for (int i = 0; i < 3; i++)
    {
        order = new Order { DateTime = DateTime.Now.AddMinutes(-i * 32) };
        order.Products = new ObservableCollection<Product>(Enumerable.Range(1, new Random().Next(2, 10)).Select(x => new Product { Name = x.ToString(), Price = x }).ToList());

        context.Add(order);
    }


    Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
    Console.WriteLine("-------");
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    context.SaveChanges();
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);

    order.DateTime = DateTime.Now;
    order.Products.First().Name = "aaa";
    //context.ChangeTracker.DetectChanges();
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);

    context.Entry(order.Products.Skip(1).First()).Property(x => x.Name).CurrentValue = "bb";
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);

    context.ChangeTracker.AutoDetectChangesEnabled = false;
    context.SaveChanges();

    order.Products.First().Price = 3;
    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    context.SaveChanges();



    context.ChangeTracker.AutoDetectChangesEnabled = true;

    order.Products.First().Name = "Car";
    order.DateTime = DateTime.Now;

    var saved = false;
    while (!saved)
    {
        try
        {
            context.SaveChanges();
            saved = true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                //wartości stanu jaki chcemy wprowadzić do bazy danych
                var currentValues = entry.CurrentValues;
                //pobieramy wartości aktualnie znajdujące się w bazie danych
                var databaseValues = entry.GetDatabaseValues();

                if (entry.Entity is Order)
                {
                    var dateTimeProperty = currentValues.Properties.SingleOrDefault(x => x.Name == nameof(Order.DateTime));
                    //var currentDateTimePropertyValue = currentValues[dateTimeProperty];
                    //var databaseDateTimePropertyValue = databaseValues[dateTimeProperty];

                    //currentValues[dateTimeProperty] = currentDateTimePropertyValue;

                    foreach (var property in currentValues.Properties)
                    {
                        currentValues[property] = entry.OriginalValues[property];
                    }
                }
                else if (entry.Entity is Product)
                {
                    var dateTimeProperty = currentValues.Properties.SingleOrDefault(x => x.Name == nameof(Product.Name));
                    var currentDateTimePropertyValue = currentValues[dateTimeProperty];
                    var databaseDateTimePropertyValue = databaseValues[dateTimeProperty];

                    currentValues[dateTimeProperty] = databaseDateTimePropertyValue.ToString() + currentDateTimePropertyValue.ToString();
                }

                entry.OriginalValues.SetValues(databaseValues);
            }
        }
    }
}

static async Task Transactions(DbContextOptionsBuilder<SqlServerContext> contextOptions)
{
    var context = new SqlServerContext(contextOptions.Options);

    context.Database.EnsureDeleted();
    context.Database.Migrate();


    var products = Enumerable.Range(100, 50).Select(x => new Product { Name = $"Product {x}" }).ToList();

    var orders = Enumerable.Range(0, 5).Select(x => new Order() { DateTime = DateTime.UtcNow.AddMinutes(-3.21 * x) }).ToList();

    context.RandomFail = true;

    using (var transaction = await context.Database.BeginTransactionAsync())
    {
        for (int i = 0; i < 5; i++)
        {
            string savepoint = i.ToString();
            await transaction.CreateSavepointAsync(savepoint);
            try
            {
                var subProducts = products.Skip(i * 10).Take(10).ToList();

                foreach (var product in subProducts)
                {
                    context.Add(product);
                    await context.SaveChangesAsync();
                }

                orders[i].Products = subProducts;
                context.Add(orders[i]);
                await context.SaveChangesAsync();
            }
            catch
            {
                await transaction.RollbackToSavepointAsync(savepoint);
                context.ChangeTracker.Clear();
            }
        }

        await transaction.CommitAsync();
    }
}

static void DoSthWithOrder(Order order)
{
    order.Products.ToList();
}
static void DoSthWithProduct(Product product)
{
    product.Orders.ToList();
}

static void Loading(DbContextOptionsBuilder<SqlServerContext> contextOptions)
{
    Order order;

    using (var context = new SqlServerContext(contextOptions.Options))
    {
        order = context.Set<Order>().First();
        //EagerLoading
        //order = context.Set<Order>().Include(x => x.Products).First();

        //ExplicitLoading
        //context.Entry(order).Collection(x => x.Products).Load();
        DoSthWithOrder(order);
    }

    Console.WriteLine();

    //LazyLoding z wykorzystanie Proxy - wszystkie referencje w modelu muszą być virtual
    using (var context = new SqlServerContext(contextOptions.Options))
    {
        var product = context.Set<Product>().First();
        DoSthWithProduct(product);
    }
    Console.WriteLine();

    using (var context = new SqlServerContext(contextOptions.Options))
    {
        var orders = Context.GetOrderFromTo(context, DateTime.Now.AddDays(-1), DateTime.Now).ToList();
        orders = Context.GetOrderFromTo(context, DateTime.Now.AddDays(-1), DateTime.Now).ToList();
    }
}

static void Procedures(DbContextOptionsBuilder<SqlServerContext> contextOptions)
{
    using (var context = new SqlServerContext(contextOptions.Options))
    {
        var products = context.Set<Product>().Where(x => EF.Property<int?>(x, "ManufacturerId").HasValue).ToList();

        context.Database.ExecuteSqlRaw("EXEC ChangePrice @p0", -1);
        context.Database.ExecuteSqlInterpolated($"EXEC ChangePrice {-10}");

        var result = context.Set<OrderSummary>().FromSqlInterpolated($"EXEC OrderSummary {1}").ToList();
        result = context.Set<OrderSummary>().ToList();
    }
}

static void ModelBuilding(DbContextOptionsBuilder<SqlServerContext> contextOptions)
{
    using (var context = new SqlServerContext(contextOptions.Options))
    {
        var orders = context.Set<Order>().ToList();
        var order = orders.First();

        order.IsDeleted = true;

        context.SaveChanges();
    }
    using (var context = new SqlServerContext(contextOptions.Options))
    {
        var orders = context.Set<Order>().ToList();
        orders = context.Set<Product>().Include(x => x.Orders).SelectMany(x => x.Orders).Distinct().ToList();

        var order = orders.First();

        context.Entry(order).Property<string>("Metadata").CurrentValue = "super zamówienie!";
        context.SaveChanges();
    }
    using (var context = new SqlServerContext(contextOptions.Options))
    {
        var products = context.Set<Product>().ToList();
        products.First().Manufacturer = new Manufacturer() { Name = "Altkom" };

        context.SaveChanges();
    }
}