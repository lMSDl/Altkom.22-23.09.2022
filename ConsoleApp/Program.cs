using DAL.SqlServer;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.ObjectModel;

var contextOptions = new DbContextOptionsBuilder<SqlServerContext>()
    .UseSqlServer(@"Server=.\SqlExpress;Database=EF6;Integrated Security=True");

var context = new SqlServerContext(contextOptions.Options);

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

var order = new Order();
var product = new Product();
order.Products.Add(product);

Console.WriteLine(  context.Entry(order).State);
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