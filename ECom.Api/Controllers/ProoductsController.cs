using AutoMapper;
using ECom.Api.Helper;
using ECom.Core.DTO;
using ECom.Core.Entities.Product;
using ECom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProoductsController : BaseController
    {
        public ProoductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await work.ProductRepository
                    .GetAllAsync(x => x.Category, x => x.Photos);
                if (products is null) return BadRequest(new ResponseAPI(400));
                var result = mapper.Map<List<ProductDTO>>(products);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await work.ProductRepository
                    .GetByIdAsync(id, x => x.Category, x => x.Photos);
                if (product is null) return BadRequest(new ResponseAPI(404));
                var result = mapper.Map<ProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> Add(AddProductDTO productDTO)
        {
            try
            {
                await work.ProductRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400,ex.Message));
            }
        }
        [HttpPut("update-product")]
        public async Task<IActionResult> Update(UpdateProductDTO productDTO)
        {
            try
            {
                await work.ProductRepository.UpdateAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpDelete("delete-product/{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            try
            {
                var product = await work.ProductRepository.
                    GetByIdAsync(Id, x => x.Photos, x => x.Category);
                await work.ProductRepository.DeleteAsync(product);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

    }
}
