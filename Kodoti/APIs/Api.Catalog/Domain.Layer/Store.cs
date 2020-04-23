using Domain.Layer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Layer
{
    public class Store : DomainBase
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public List<ProductStore> ProductStores { get; set; }
    }
}