using ECom.Core.DTO;
using ECom.Core.Entities.Product;
using ECom.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(AddProductDTO productDTO);
        Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO);
        Task DeleteAsync(Product product);
        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParam productParams);
    }
}
