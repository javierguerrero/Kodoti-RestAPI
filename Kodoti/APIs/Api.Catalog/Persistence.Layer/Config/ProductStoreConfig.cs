using Domain.Layer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Layer.Config
{
    public class ProductStoreConfig
    {
        public ProductStoreConfig(EntityTypeBuilder<ProductStore> entityBuilder)
        {

        }
    }
}
