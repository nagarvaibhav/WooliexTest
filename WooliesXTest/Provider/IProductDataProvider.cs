using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesXTest.DTO;

namespace WooliesXTest.Provider
{
    public interface IProductDataProvider
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<ShoppersHistory>> GetShoppersHistory();
    }
}
