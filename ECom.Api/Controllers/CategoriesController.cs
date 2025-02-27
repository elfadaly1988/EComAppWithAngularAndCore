using AutoMapper;
using ECom.Api.Helper;
using ECom.Core.DTO;
using ECom.Core.Entities.Product;
using ECom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ECom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
                var cateory = await work.CategoryRepository.GetAllAsync();
                if (cateory is null) return BadRequest(new ResponseAPI(400));
                return Ok(cateory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await work.CategoryRepository.GetByIdAsync(id);
                if (category is null) return BadRequest(new ResponseAPI(400,$"not found category {id}"));
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> Add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await work.CategoryRepository.AddAsync(category);
                return Ok(new ResponseAPI(200,"Item has been added"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult> Update(UpdateCategoryDTO updatecategoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(updatecategoryDTO);

                await work.CategoryRepository.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Item has been updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                await work.CategoryRepository.DeleteAsync(id);
                return Ok(new ResponseAPI(200, "Item has been deleted"));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
