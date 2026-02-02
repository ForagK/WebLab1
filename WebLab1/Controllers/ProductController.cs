using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebLab1.Models;
using WebLab1.Services;

namespace WebLab1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            var _product = _service.CreateProduct(product);
            return CreatedAtAction("GetProduct", new { id = _product.Id }, _product);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (!_service.DeleteProduct(id))
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _service.GetProduct(id);
            if (product == null)
                return NotFound();

            var eTag = _service.GenerateETag(product);

            if (Request.Headers.TryGetValue("If-None-Match", out var clientETag))
            {
                if (clientETag == eTag)
                {
                    return StatusCode(StatusCodes.Status304NotModified);
                }
            }

            Response.Headers.ETag = eTag;

            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            if (_service.UpdateProduct(id, product))
                return NoContent();
            return NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchProduct(int id, Dictionary<string, object> newProduct)
        {
            if (_service.PatchProduct(id, newProduct))
                return NoContent();
            return NotFound();
        }

        [HttpHead("{id}")]
        public IActionResult Head(int id)
        {
            return _service.GetProduct(id) != null ? Ok() : NotFound();
        }
    }
}
