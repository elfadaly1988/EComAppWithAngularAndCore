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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(maxLength: 30);
            builder.Property(p => p.Id).IsRequired();
            builder.HasData(new Category
            {
                Id = 1,
                Name = "Test",
                Description = "Test"
            });            
        }
    }
}
