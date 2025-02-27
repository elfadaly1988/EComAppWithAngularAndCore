using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entities.Product;

namespace ECom.Api.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UpdateCategoryDTO, Category>().ReverseMap();
        }
    }
}
