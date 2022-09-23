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
    internal class ShadowIdConfiguration : IEntityTypeConfiguration<ShadowId>
    {
        public void Configure(EntityTypeBuilder<ShadowId> builder)
        {
            builder.Property<int>("Id");
            builder.HasKey("Id");
        }
    }
}
