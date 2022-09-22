using DAL.SqlServer;
using Microsoft.EntityFrameworkCore;

var contextOptions = new DbContextOptionsBuilder<SqlServerContext>()
    .UseSqlServer(@"Server=.\SqlExpress;Database=EF6;Integrated Security=True");

var context = new SqlServerContext(contextOptions.Options);

context.Database.EnsureDeleted();
context.Database.EnsureCreated();