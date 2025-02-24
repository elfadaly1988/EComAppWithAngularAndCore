using ECom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(propertyExpression: p => p.Name).IsRequired();
            builder.Property(propertyExpression: p => p.Description).IsRequired();
            builder.Property(propertyExpression: p => p.Price).HasColumnType(typeName: "decimal(18,2)").IsRequired();
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Test",
                    Price = 100,                     
                    CategoryId = 1
                }
                );
        }
    }
}
