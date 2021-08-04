using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.DTO;
using WooliesXTest.Infrastructure.SortStrategies.Interfaces;
using WooliesXTest.Provider;

namespace WooliesXTest.Infrastructure.SortStrategies
{
    public class ProductsPriceAscending : IProductsSortStrategy
    {
        private readonly IProductDataProvider _productDataProvider;

        public ProductsPriceAscending(IProductDataProvider productDataProvider)
        {
            _productDataProvider = productDataProvider;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _productDataProvider.GetProducts();
            return products.OrderBy(x => x.Price);
        }
    }
}
