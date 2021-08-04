using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.DTO;
using WooliesXTest.Infrastructure.SortStrategies.Interfaces;
using WooliesXTest.Provider;

namespace WooliesXTest.Infrastructure.SortStrategies
{
    public class ProductsPriceDecending : IProductsSortStrategy
    {
        private readonly IProductDataProvider _productDataProvider;

        public ProductsPriceDecending(IProductDataProvider productDataProvider)
        {
            _productDataProvider = productDataProvider;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _productDataProvider.GetProducts();
            return products.OrderByDescending(x => x.Price);
        }
    }
}
