using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    internal class ProductConfiguration : EntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsConcurrencyToken();
            builder.Ignore(x => x.Desc);

            builder.Property(x => x.Name).HasConversion(x => Convert.ToBase64String(Encoding.UTF8.GetBytes(x)),
                                                         x => Encoding.UTF8.GetString(Convert.FromBase64String(x)));
        }
    }
}
