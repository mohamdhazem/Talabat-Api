using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.PictureUrl)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Relation between Brand <=> Product (1 to M)
            builder.HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId);

            // Relation between Category <=> Product (1 to M)
            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

        }
    }
}
