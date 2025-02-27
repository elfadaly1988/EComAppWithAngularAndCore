using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entities.Product;
namespace ECom.Api.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>().ForMember(x => x.CategoryName, op =>
            op.MapFrom(src => src.Category.Name)).ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<AddProductDTO, Product>()
                .ForMember(m => m.Photos, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(m => m.Photos, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
