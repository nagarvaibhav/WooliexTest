using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.DTO;
using WooliesXTest.Infrastructure.SortStrategies.Interfaces;
using WooliesXTest.Provider;
using WooliesXTest.Utility;

namespace WooliesXTest.Infrastructure.SortStrategies
{
    public class RecommendedProducts : IProductsSortStrategy
    {
        private readonly IProductDataProvider _productDataProvider;

        public RecommendedProducts(IProductDataProvider productDataProvider)
        {
            _productDataProvider = productDataProvider;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _productDataProvider.GetProducts();
            var shopersList = await _productDataProvider.GetShoppersHistory();
            var recommendedProducts = shopersList.SelectMany(x => x.Products)
                                        .GroupBy(x => x.Name)
                                        .OrderByDescending(x => x.Sum(y => y.Quantity))
                                        .Select(x => new Product { Name = x.First().Name, Price = x.First().Price }).ToList();

            var reaminingProducts = products.Where(x => !recommendedProducts.Contains(x, new ProductComparer()));
            recommendedProducts.AddRange(reaminingProducts);
            return recommendedProducts;
        }
    }
}
