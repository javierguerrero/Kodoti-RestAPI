using Domain.Layer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Layer
{
    public class Product : DomainBase
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public List<ProductStore> ProductStores { get; set; }
    }
}
