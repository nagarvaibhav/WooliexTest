using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesXTest.DTO;

namespace WooliesX.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> SortProducts(string sortOption);
        decimal GetTrolleyTotal(TrolleyRequest request);
    }
}
