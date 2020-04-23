using Domain.Layer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Layer
{
    public class ProductStore : DomainBase
    {
        public int ProductStoreId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
