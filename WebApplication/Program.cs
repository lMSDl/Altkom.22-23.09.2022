using DAL.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<SqlServerContext>(x => x.UseSqlServer(""));
builder.Services.AddDbContextPool<SqlServerContext>(x => x.UseSqlServer(""), 64);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
