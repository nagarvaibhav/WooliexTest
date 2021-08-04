using System.Collections.Generic;

namespace WooliesXTest.DTO
{
    public class TrolleyRequest
    {
        public IEnumerable<TrolleyProduct> Products { get; set; }
        public IEnumerable<Special> Specials { get; set; }
        public IEnumerable<ProductQuantity> Quantities { get; set; }
    }
    public class TrolleyProduct
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public class ProductQuantity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class Special
    {
        public IEnumerable<ProductQuantity> Quantities { get; set; }
        public int Total { get; set; }
    }
}
