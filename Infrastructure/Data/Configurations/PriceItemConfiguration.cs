using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class PriceItemConfiguration : IEntityTypeConfiguration<PriceItem>
    {
        public void Configure(EntityTypeBuilder<PriceItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(i => i.Vendor)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(i => i.Number)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(i => i.SearchVendor)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(i => i.SearchNumber)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(i => i.Description)
                .HasMaxLength(512);
            builder.Property(i => i.Price)
                .HasPrecision(18, 2);
            builder.Property(i => i.Count)
                .IsRequired();
        }
    }
}
