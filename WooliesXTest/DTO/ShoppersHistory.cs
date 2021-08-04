using System.Collections.Generic;

namespace WooliesXTest.DTO
{
    public class ShoppersHistory
    {
        public IEnumerable<Product> Products { get; set; }
        public int CustomerId { get; set; }
    }
}
