using Microsoft.EntityFrameworkCore;

namespace DAL.SQLite
{
    public class SQLiteContext : Context
    {
        public SQLiteContext()
        {
        }

        public SQLiteContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite();

            base.OnConfiguring(optionsBuilder);
        }

    }
}