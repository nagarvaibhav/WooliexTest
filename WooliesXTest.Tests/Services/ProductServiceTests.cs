using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.DTO;
using WooliesXTest.Infrastructure.SortStrategies;
using WooliesXTest.Infrastructure.SortStrategies.Interfaces;
using WooliesXTest.Provider;
using WooliesXTest.Services;
using WooliesXTest.Utility;

namespace WooliesXTest.Tests.Services
{
    public class ProductServiceTests
    {
        private IProductDataProvider _productDataProvider;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _productDataProvider = Substitute.For<IProductDataProvider>();
            Func<string, IProductsSortStrategy> productSortStrategies = delegate (string key)
            {
                switch (key)
                {
                    case Constants.Low:
                        return new ProductsPriceAscending(_productDataProvider);
                    case Constants.High:
                        return new ProductsPriceDecending(_productDataProvider);
                    case Constants.Ascending:
                        return new ProductsNameAscending(_productDataProvider);
                    case Constants.Descending:
                        return new ProductsNameDecending(_productDataProvider);
                    case Constants.Recommended:
                        return new RecommendedProducts(_productDataProvider);
                    default:
                        throw new Exception("Invalid Sort Option");
                }
            };
            _productService = new ProductService(_productDataProvider, productSortStrategies);
        }

        [Test]
        public async Task SortProducts_Should_Return_Sorted_Products_BasedOn_SortOptions()
        {
            var products = MockDataProvider.GetProducts();
            _productDataProvider.GetProducts().Returns(products);

            var resultLow = (await _productService.SortProducts(Constants.Low)).ToList();
            Assert.AreEqual(3, resultLow.Count);
            Assert.AreEqual(43, resultLow[0].Price);
            Assert.AreEqual("B Test Product", resultLow[0].Name);
            var resultHigh = (await _productService.SortProducts(Constants.High)).ToList();
            Assert.AreEqual(3, resultHigh.Count);
            Assert.AreEqual(51, resultHigh[0].Price);
            Assert.AreEqual("C Test Product", resultHigh[0].Name);
            var resultAscending = (await _productService.SortProducts(Constants.Ascending)).ToList();
            Assert.AreEqual(3, resultAscending.Count);
            Assert.AreEqual(45, resultAscending[0].Price);
            Assert.AreEqual("A Test Product", resultAscending[0].Name);
            var resultDescending = (await _productService.SortProducts(Constants.Descending)).ToList();
            Assert.AreEqual(3, resultDescending.Count);
            Assert.AreEqual(51, resultDescending[0].Price);
            Assert.AreEqual("C Test Product", resultDescending[0].Name);
        }

        [Test]
        public async Task SortProducts_Should_Return_Sorted_Products_BasedOn_Recommended_Products()
        {

            var products = MockDataProvider.GetProducts();
            _productDataProvider.GetProducts().Returns(products);

            var shoppersHistory = MockDataProvider.GetShoppersHistory();
            _productDataProvider.GetShoppersHistory().Returns(shoppersHistory);

            var result = (await _productService.SortProducts(Constants.Recommended)).ToList();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(45, result[0].Price);
            Assert.AreEqual("A Test Product", result[0].Name);
            Assert.AreEqual(51, result[1].Price);
            Assert.AreEqual("B Test Product", result[1].Name);
            Assert.AreEqual(51, result[2].Price);
            Assert.AreEqual("C Test Product", result[2].Name);
        }

        [Test]
        public void GetTrolleyTotal_Should_Return_TrolleyTotal_Considering_Specials_For_Valid_Request()
        {
            var trolleyRequest = MockDataProvider.GetTrolleyRequest(5, 3);

            var result = _productService.GetTrolleyTotal(trolleyRequest);
            Assert.AreEqual(102, result);
        }

        [Test]
        public void GetTrolleyTotal_Should_Return_TrolleyTotal_Ignoring_Specials_If_ProductQuantity_IsLessThan_Special_Quantity()
        {
            var trolleyRequest = MockDataProvider.GetTrolleyRequest(2, 3);

            var result = _productService.GetTrolleyTotal(trolleyRequest);
            Assert.AreEqual(100, result);
        }

        [Test]
        public void GetTrolleyTotal_Should_Return_TrolleyTotal_Ignoring_Specials_If_Special_Is_NotPresent()
        {
            var trolleyRequest = MockDataProvider.GetTrolleyRequest(2, 3, true);

            var result = _productService.GetTrolleyTotal(trolleyRequest);
            Assert.AreEqual(100, result);
        }

        [TestCase(null)]
        [TestCase("Product")]
        [TestCase("Quantity")]
        public void GetTrolleyTotal_Should_Return_0_ForInvalid_Request_When_Request_Is_Null(string paramName)
        {
            var trolleyRequest = MockDataProvider.GetTrolleyRequest(2, 3, true);
            if (string.Compare(paramName, "Product", true) == 0)
                trolleyRequest.Products = null;
            else
                trolleyRequest.Quantities = null;

            Assert.AreEqual(0, _productService.GetTrolleyTotal(trolleyRequest));
        }

        [Test]

        public void GetTrolleyTotal_Should_Return_0_ForInvalid_Request_When_No_ProductInfo_Present()
        {
            var trolleyRequest = MockDataProvider.GetTrolleyRequest(2, 3);
            trolleyRequest.Products = new List<TrolleyProduct>();
            Assert.AreEqual(0,_productService.GetTrolleyTotal(trolleyRequest));
        }

        [Test]
        public void GetTrolleyTotal_Should_Return_0_ForInvalid_Request_When_No_QuantityInfo_Present()
        {
            var trolleyRequest = MockDataProvider.GetTrolleyRequest(2, 3);
            trolleyRequest.Quantities = new List<ProductQuantity>();
            Assert.AreEqual(0, _productService.GetTrolleyTotal(trolleyRequest));
        }

        [Test]
        public void GetTrolleyTotal_Should_Return_0_ForInvalid_Request_When_ProductName_Does_Not_Match_InQuantity()
        {
            var trolleyRequest = MockDataProvider.GetTrolleyRequest(2, 3);
            trolleyRequest.Quantities.First().Name = "Test";
            Assert.AreEqual(0, _productService.GetTrolleyTotal(trolleyRequest));
        }
    }
}
