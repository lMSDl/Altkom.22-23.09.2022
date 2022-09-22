﻿using DAL.SqlServer;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

var contextOptions = new DbContextOptionsBuilder<SqlServerContext>()
    .LogTo(x => Debug.WriteLine(x))
    //LazyLoading
    .UseLazyLoadingProxies()
    .UseSqlServer(@"Server=.\SqlExpress;Database=EF6;Integrated Security=True");

await Transactions(contextOptions);

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
    //EagerLoading
    //order = context.Set<Order>().Include(x => x.Products).First();

    //ExplicitLoading
    //context.Entry(order).Collection(x => x.Products).Load();
    DoSthWithProduct(product);
}
Console.WriteLine();


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


    var products = Enumerable.Range(100, 50).Select(x => new Product { Name = $"Product {x}", Price = 1.23f * x }).ToList();

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