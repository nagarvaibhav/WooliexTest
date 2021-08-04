using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesX.Services.Interfaces;
using WooliesXTest.Controllers;
using WooliesXTest.DTO;

namespace WooliesXTest.Tests.Controller
{
    public class ProductControllerTests
    {
        private IProductService _productService;
        private ProductController _productController;
        private ILogger<ProductController> _logger;

        [SetUp]
        public void SetUp()
        {
            _productService = Substitute.For<IProductService>();
            _logger = Substitute.For<ILogger<ProductController>>();
            _productController = new ProductController(_productService, _logger);
        }

        [TestCase("Test")]
        public async Task GetProduct_Should_Return_SucessFull_Response(string sortOption)
        {
            var products = MockDataProvider.GetProducts();
            _productService.SortProducts(sortOption).Returns(products);
            var result = await _productController.GetProducts(sortOption) as OkObjectResult;
            var productsResponse = result.Value as List<Product>;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(productsResponse);
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.AreEqual(3, productsResponse.Count);
        }


        [Test]
        public async Task GetProduct_Should_Return_BadRequest_Response_For_Empty_SortOption()
        {
            var result = await _productController.GetProducts("") as BadRequestObjectResult;
            var productsResponse = result.Value;
            await _productService.DidNotReceive().SortProducts("");
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsNotNull(productsResponse);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.AreEqual("Empty Sort Option", productsResponse);
        }

        [Test]
        public async Task GetProduct_Should_Return_BadRequest_Response_For_Invalid_SortOption()
        {
            List<Product> products = null;
            _productService.SortProducts("test").Returns(products);
            var result = await _productController.GetProducts("test") as BadRequestObjectResult;
            var productsResponse = result.Value;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsNotNull(productsResponse);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.AreEqual("Invalid Sort Option. Valid values are Low,High,ascending,descending,recommended", productsResponse);
        }

        [Test]
        public async Task GetProduct_Should_Return_Error_Response_For_AnyException()
        {
            _productService.When(x => x.SortProducts("test")).Do(x => { throw new Exception("Error"); });
            var result = await _productController.GetProducts("test") as ObjectResult;
            var productsResponse = result.Value;
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsNotNull(productsResponse);
            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual("Error in getting products", productsResponse);
        }

        [Test]
        public void TrolleyTotal_Should_Return_SucessFull_Response_For_Valid_Request()
        {
            var request = MockDataProvider.GetTrolleyRequest(1, 2);
            _productService.GetTrolleyTotal(request).Returns(5);
            var result = _productController.TrolleyTotal(request) as OkObjectResult;
            var trolleyTotalResponse = result.Value;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(trolleyTotalResponse);
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.AreEqual(5, trolleyTotalResponse);
        }

        [Test]
        public void TrolleyTotal_Should_Return_BadRequest_Response_For_InValid_Request_When_No_Product_Info_Present()
        {
            var request = MockDataProvider.GetTrolleyRequest(1, 2);
            request.Products = null;
            var result = _productController.TrolleyTotal(request) as BadRequestObjectResult;
            var trolleyTotalResponse = result.Value;
            _productService.DidNotReceive().GetTrolleyTotal(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsNotNull(trolleyTotalResponse);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.AreEqual("Please add Product", trolleyTotalResponse);
        }

        [Test]
        public void TrolleyTotal_Should_Return_BadRequest_Response_For_InValid_Request_When_No_Quantity_Info_Present()
        {
            var request = MockDataProvider.GetTrolleyRequest(1, 2);
            request.Quantities = null;
            var result = _productController.TrolleyTotal(request) as BadRequestObjectResult;
            var trolleyTotalResponse = result.Value;
            _productService.DidNotReceive().GetTrolleyTotal(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsNotNull(trolleyTotalResponse);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.AreEqual("Please add Product Quantity", trolleyTotalResponse);
        }

        [Test]
        public void TrolleyTotal_Should_Return_BadRequest_Response_For_InValid_Request_When_ProductName_Does_Not_Match_InQuantity()
        {
            var request = MockDataProvider.GetTrolleyRequest(1, 2);
            request.Quantities.First().Name = "Test";
            var result = _productController.TrolleyTotal(request) as BadRequestObjectResult;
            var trolleyTotalResponse = result.Value;
            _productService.DidNotReceive().GetTrolleyTotal(request);
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsNotNull(trolleyTotalResponse);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.AreEqual("Please add Quantity for product: " + request.Products.First().Name, trolleyTotalResponse);
        }

        [Test]
        public void TrolleyTotal_Should_Return_Error_Response_For_AnyException()
        {
            var request = MockDataProvider.GetTrolleyRequest(1, 2);
            _productService.When(x => x.GetTrolleyTotal(request)).Do(x => { throw new Exception("Error"); });
            var result = _productController.TrolleyTotal(request) as ObjectResult;
            var trolleyResponse = result.Value;
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsNotNull(trolleyResponse);
            Assert.IsInstanceOf(typeof(ObjectResult), result);
            Assert.AreEqual("Error in calculating trolley total", trolleyResponse);
        }
    }
}
