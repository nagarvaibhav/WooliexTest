using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WooliesX.Services.Interfaces;
using WooliesXTest.DTO;

namespace WooliesXTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [Route("Sort")]
        public async Task<IActionResult> GetProducts(string sortOption)
        {
            try
            {
                if (string.IsNullOrEmpty(sortOption))
                    return BadRequest("Empty Sort Option");

                var result = await _productService.SortProducts(sortOption);
                
                if (result == null)
                {
                    return BadRequest("Invalid Sort Option. Valid values are Low,High,ascending,descending,recommended");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting products");
                return StatusCode(500, "Error in getting products");
            }
        }

        [HttpPost]
        [Route("trolleyTotal")]
        public IActionResult TrolleyTotal([FromBody] TrolleyRequest request)
        {
            try
            {
                if (request.Products == null || !request.Products.Any())
                    return BadRequest("Please add Product");

                if (request.Quantities == null || !request.Quantities.Any())
                    return BadRequest("Please add Product Quantity");

                var productName = request.Products.First().Name;

                if (!request.Quantities.Select(x => x.Name.Equals(productName)).First())
                {
                    return BadRequest("Please add Quantity for product: " + productName);
                }

                var result = _productService.GetTrolleyTotal(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in calculating trolley total");
                return StatusCode(500, "Error in calculating trolley total");
            }
        }
    }
}
