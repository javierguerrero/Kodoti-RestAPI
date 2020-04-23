using AutoMapper;
using Common.Layer;
using Domain.Dto.Layer;
using Domain.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Config
{
    public static class AutoMapperConfig
    {
        public static void Config()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<ProductDto, Product>();
                cfg.CreateMap<ProductCreateDto, Product>();
                cfg.CreateMap<ProductUpdateDto, Product>();
                cfg.CreateMap<DataCollection<ProductDto>, DataCollection<Product>>();

                cfg.CreateMap<StoreDto, Store>();
                cfg.CreateMap<ProductStoreDto, ProductStore>();
            });
        }
    }
}
