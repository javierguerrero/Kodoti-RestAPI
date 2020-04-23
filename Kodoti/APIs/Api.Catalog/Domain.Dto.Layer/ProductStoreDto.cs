using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto.Layer
{
    public class ProductStoreDto
    {
        public int ProductStoreId { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public StoreDto Store { get; set; }
    }
}
