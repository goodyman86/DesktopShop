using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using DesktopShop.Application.DTOs.Category;
using DesktopShop.Application.Interfaces;

namespace DesktopShop.API.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("active")]
        public async Task<IHttpActionResult> GetActive()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var category = await _categoryService.CreateCategoryAsync(dto);
            return Created(new Uri(Request.RequestUri, "api/categories/" + category.Id), category);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (id != dto.Id) return BadRequest("ID không khớp.");
            var category = await _categoryService.UpdateCategoryAsync(dto);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result) return NotFound();
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
