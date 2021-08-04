using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesXTest.DTO;

namespace WooliesXTest.Infrastructure.SortStrategies.Interfaces
{
    public interface IProductsSortStrategy
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
