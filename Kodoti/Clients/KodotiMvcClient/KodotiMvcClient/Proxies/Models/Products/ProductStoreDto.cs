using KodotiMvcClient.Proxies.Models.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodotiMvcClient.Proxies.Models.Products
{
    public class ProductStoreDto
    {
        public int ProductStoreId { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public StoreDto Store { get; set; }
    }
}
