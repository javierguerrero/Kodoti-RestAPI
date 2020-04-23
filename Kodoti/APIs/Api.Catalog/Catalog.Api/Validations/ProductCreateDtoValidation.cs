using Domain.Dto.Layer;
using FluentValidation;
using Services.Layer;

namespace Catalog.Api.Validations
{
    public class ProductCreateDtoValidation : AbstractValidator<ProductCreateDto>
    {
        private readonly IProductService _productService;

        public ProductCreateDtoValidation(IProductService productService)
        {
            _productService = productService;

            RuleFor(property => property.Name)
                .NotNull()
                .MinimumLength(5)
                .WithMessage("Debe contener al menos 5 caracteres")
                .Must(UniqueName)
                .WithMessage("Este nombre ya existe en la base de datos.");

            RuleFor(property => property.Description)
                .NotNull()
                .MinimumLength(10);
        }

        private bool UniqueName(string name)
        {
            return !_productService.UniqueName(name).Result;
        }
    }
}