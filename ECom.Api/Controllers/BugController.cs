using AutoMapper;
using ECom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.Api.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound()
        {
            var category = await work.CategoryRepository.GetByIdAsync(100);
            if (category is null) return NotFound();
            return Ok(category);
        }
        [HttpGet("server-error")]
        public async Task<ActionResult> GetServerError()
        {
            var category = await work.CategoryRepository.GetByIdAsync(100);
            category.Name = "";//Throw exception
            return Ok(category);
        }

        [HttpGet("bad-request/{Id}")]
        public async Task<ActionResult> GetBadRequest(int id)
        {
            var category = await work.CategoryRepository.GetByIdAsync(100);
            category.Name = "";//Throw exception
            return Ok(category);
        }
        [HttpGet("bad-request/")]
        public async Task<ActionResult> GetBadRequest()
        {
            
            return BadRequest();
        }
    }
}
