using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesX.Services.Interfaces;
using WooliesXTest.DTO;
using WooliesXTest.Infrastructure.SortStrategies.Interfaces;
using WooliesXTest.Provider;
using WooliesXTest.Utility;

namespace WooliesXTest.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDataProvider _productDataProvider;
        private readonly Func<string, IProductsSortStrategy> _sortStrategy;

        public ProductService(IProductDataProvider productDataProvider, Func<string, IProductsSortStrategy> sortStrategy)
        {
            _productDataProvider = productDataProvider;
            _sortStrategy = sortStrategy;
        }

        public async Task<IEnumerable<Product>> SortProducts(string sortOption)
        {
            return await _sortStrategy(sortOption).GetProducts();
        }

        public decimal GetTrolleyTotal(TrolleyRequest request)
        {
            if (!request.Products.HasItems() || !request.Quantities.HasItems())
            {
                return 0;
            }

            var product = request.Products.FirstOrDefault();
            var productQuantity = request.Quantities.FirstOrDefault();
            
            if (string.Compare(product.Name, productQuantity.Name, false) != 0)
                return 0;
            
            var specialQuantity = 0;
            var specialTotal = 0;

            if (request.Specials.HasItems())
            {
                var special = request.Specials.FirstOrDefault();
                if (special != null && special.Quantities != null && special.Quantities.Any())
                {
                    specialQuantity = special.Quantities.Where(x => x.Name.Equals(product.Name)).Select(x => x.Quantity).First();
                    specialTotal = special.Total;
                }
            }
            if (specialQuantity < productQuantity.Quantity && specialQuantity > 0)
            {
                return ((productQuantity.Quantity % specialQuantity * product.Price)) + specialTotal * Convert.ToInt32((productQuantity.Quantity / specialQuantity));
            }
            else
            {
                return productQuantity.Quantity * product.Price;
            }
        }
    }
}
