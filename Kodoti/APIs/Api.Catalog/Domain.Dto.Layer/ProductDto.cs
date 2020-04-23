using System.Collections.Generic;

namespace Domain.Dto.Layer
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

    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class ProductPartialDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsEnabled { get; set; }
    }
}