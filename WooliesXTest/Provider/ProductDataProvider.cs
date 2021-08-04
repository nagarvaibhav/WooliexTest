using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WooliesXTest.DTO;
using WooliesXTest.Options;
using WooliesXTest.Utility;

namespace WooliesXTest.Provider
{
    public class ProductDataProvider : IProductDataProvider
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<ProductDataProvider> _logger;
        private readonly WooliexApiOptions _wooliexApiOptions;

        public ProductDataProvider(IHttpClientFactory httpClient,
            ILogger<ProductDataProvider> logger,
            IOptions<WooliexApiOptions> wooliexApiOptions)
        {
            _httpClient = httpClient;
            _logger = logger;
            _wooliexApiOptions = wooliexApiOptions.Value;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                using (var client = _httpClient.CreateClient())
                {
                    var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(_wooliexApiOptions.ApiBaseEndpoint).CombineUri(_wooliexApiOptions.ProductEndpoint).CombineUri("?token=" + _wooliexApiOptions.Token));
                    var httpResponse = await client.SendAsync(httpRequest);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return await httpResponse.Content.ReadAsAsync<IEnumerable<Product>>();
                    }
                    else
                    {
                        _logger.LogWarning("Wooliesx product resource Api Returns unsuccessful status. Code: " + httpResponse.StatusCode, httpRequest);
                        throw new Exception("Wooliesx product resource Api Returns unsuccessful status.Code: " + httpResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in calling Wooliesx product resource Api");
                throw ex;
            }
        }

        public async Task<IEnumerable<ShoppersHistory>> GetShoppersHistory()
        {
            try
            {
                using (var client = _httpClient.CreateClient())
                {
                    var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(_wooliexApiOptions.ApiBaseEndpoint).CombineUri(_wooliexApiOptions.ShoppersListEndpoint).CombineUri("?token=" + _wooliexApiOptions.Token));
                    var httpResponse = await client.SendAsync(httpRequest);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return await httpResponse.Content.ReadAsAsync<IEnumerable<ShoppersHistory>>();
                    }
                    else
                    {
                        _logger.LogWarning("Wooliesx ShoppersHistory resource  Api Returns unsuccessful status. Code: " + httpResponse.StatusCode, httpRequest);
                        throw new Exception("Wooliesx ShoppersHistory resource Api Returns unsuccessful status.Code: " + httpResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in calling Wooliesx ShoppersHistory resource Api");
                throw ex;
            }
        }

    }
}
