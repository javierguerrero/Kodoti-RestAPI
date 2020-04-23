using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KodotiMvcClient.Proxies.Models.Products
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public List<ProductStoreDto> ProductStores { get; set; }
    }

    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public List<int> Stores { get; set; }
    }
}
