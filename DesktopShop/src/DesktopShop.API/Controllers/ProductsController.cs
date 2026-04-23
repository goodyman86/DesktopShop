using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DesktopShop.Application.DTOs.Product;
using DesktopShop.Application.Interfaces;

namespace DesktopShop.API.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet]
        [Route("category/{categoryId:int}")]
        public async Task<IHttpActionResult> GetByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet]
        [Route("low-stock")]
        public async Task<IHttpActionResult> GetLowStock()
        {
            var products = await _productService.GetLowStockProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return Ok(new ProductDto[0]);
            var products = await _productService.SearchProductsAsync(keyword);
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] CreateProductDto dto)
        {
            var product = await _productService.CreateProductAsync(dto);
            return Created(new Uri(Request.RequestUri, "api/products/" + product.Id), product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            if (id != dto.Id) return BadRequest("ID không khớp.");
            var product = await _productService.UpdateProductAsync(dto);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result) return NotFound();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("upload-image")]
        public async Task<IHttpActionResult> UploadImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Không có file nào được chọn.");

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.FirstOrDefault();
            if (file == null || file.Headers.ContentLength == 0)
                return BadRequest("Không có file nào được chọn.");

            var fileName = file.Headers.ContentDisposition.FileName?.Trim('"') ?? "upload.jpg";
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .webp).");

            var newFileName = $"{Guid.NewGuid()}{ext}";
            var uploadsFolder = HttpContext.Current.Server.MapPath("~/images/products");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, newFileName);
            var bytes = await file.ReadAsByteArrayAsync();
            File.WriteAllBytes(filePath, bytes);

            var imageUrl = $"/images/products/{newFileName}";
            return Ok(new { imageUrl });
        }
    }
}
