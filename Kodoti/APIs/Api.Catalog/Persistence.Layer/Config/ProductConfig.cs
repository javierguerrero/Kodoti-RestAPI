using Domain.Layer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Layer.Config
{
    public class ProductConfig
    {
        public ProductConfig(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entityBuilder.Property(x => x.Description).IsRequired().HasMaxLength(500);

        }
    }
}
