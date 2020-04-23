using Domain.Layer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Layer.Config
{
    public class StoreConfig
    {
        public StoreConfig(EntityTypeBuilder<Store> entityBuilder)
        {
            entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        }
    }
}
