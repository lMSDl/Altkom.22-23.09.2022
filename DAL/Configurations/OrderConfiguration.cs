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
    internal class OrderConfiguration : EntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Timestamp).IsRowVersion().UsePropertyAccessMode(PropertyAccessMode.PreferProperty);

            builder.Property(x => x.OrderType)/*.HasConversion(x => x.ToString(),
                                                             x => (OrderTypes)Enum.Parse(typeof(OrderTypes),x))*/
                                                /*.HasConversion(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.EnumToStringConverter<OrderTypes>())*/
                                                .HasConversion<string>();

        }
    }
}
