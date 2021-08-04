using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WooliesXTest.Options;
using WooliesXTest.Provider;

namespace WooliesXTest.Tests.Provider
{
    public class ProductDataProviderTests
    {
        private ProductDataProvider _productDataProvider;
        private ILogger<ProductDataProvider> _logger;
        private IHttpClientFactory _httpClient;
        private IOptions<WooliexApiOptions> _wooliexApiOptions;

        [SetUp]
        public void SetUp()
        {
            _httpClient = Substitute.For<IHttpClientFactory>();
            _logger = Substitute.For<ILogger<ProductDataProvider>>();
            _wooliexApiOptions = Microsoft.Extensions.Options.Options.Create(new WooliexApiOptions
            {
                ApiBaseEndpoint = "http://dev-wooliesx-recruitment.azurewebsites.net",
                ProductEndpoint = "test",
                ShoppersListEndpoint = "test",
                Token = "test"
            });
            _productDataProvider = new ProductDataProvider(_httpClient, _logger, _wooliexApiOptions);
        }

        [Test]
        public async Task Getproduct_Should_Return_Response_Sucessfully()
        {
            var products = MockDataProvider.GetProducts();
            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            _httpClient.CreateClient().Returns(fakeHttpClient);

            var result = (await _productDataProvider.GetProducts()).ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("A Test Product", result[0].Name);
            Assert.AreEqual(45, result[0].Price);
            Assert.AreEqual("C Test Product", result[1].Name);
            Assert.AreEqual(51, result[1].Price);
            Assert.AreEqual("B Test Product", result[2].Name);
            Assert.AreEqual(43, result[2].Price);
        }

        [Test]
        public void Getproduct_Should_Throw_Exception_For_UnSucessfull_Api_Call()
        {
            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Fake Error", Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            _httpClient.CreateClient().Returns(fakeHttpClient);

            var result = Assert.ThrowsAsync<Exception>(async () => await _productDataProvider.GetProducts());
            Assert.AreEqual("Wooliesx product resource Api Returns unsuccessful status.Code: "+ HttpStatusCode.InternalServerError, result.Message);
        }

        [Test]
        public void Getproduct_Should_Throw_Exception_For_Any_Exception()
        {
            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Fake Error", Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            _httpClient.When(x => x.CreateClient()).Do(x => { throw new Exception("Fake Exception"); });

            var result = Assert.ThrowsAsync<Exception>(async () => await _productDataProvider.GetProducts());
            Assert.AreEqual("Fake Exception", result.Message);
        }

        [Test]
        public async Task GetShoppersHistory_Should_Return_Response_Sucessfully()
        {
            var shoppersHistory = MockDataProvider.GetShoppersHistory();
            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(shoppersHistory), Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            _httpClient.CreateClient().Returns(fakeHttpClient);

            var result = (await _productDataProvider.GetShoppersHistory()).ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].Products.Count());
            Assert.AreEqual(123, result[0].CustomerId);
            Assert.AreEqual(1, result[1].Products.Count());
            Assert.AreEqual(23, result[1].CustomerId);
            Assert.AreEqual(1, result[2].Products.Count());
            Assert.AreEqual(54, result[2].CustomerId);
        }

        [Test]
        public void GetShoppersHistory_Should_Throw_Exception_For_UnSucessfull_Api_Call()
        {
            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Fake Error", Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            _httpClient.CreateClient().Returns(fakeHttpClient);

            var result = Assert.ThrowsAsync<Exception>(async () => await _productDataProvider.GetShoppersHistory());
            Assert.AreEqual("Wooliesx ShoppersHistory resource Api Returns unsuccessful status.Code: " + HttpStatusCode.InternalServerError, result.Message);
        }

        [Test]
        public void GetShoppersHistory_Should_Throw_Exception_For_Any_Exception()
        {
            var fakeHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Fake Error", Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);

            _httpClient.When(x => x.CreateClient()).Do(x => { throw new Exception("Fake Exception"); });

            var result = Assert.ThrowsAsync<Exception>(async () => await _productDataProvider.GetShoppersHistory());
            Assert.AreEqual("Fake Exception", result.Message);
        }


    }
}
