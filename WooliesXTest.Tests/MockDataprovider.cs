using System.Collections.Generic;
using WooliesXTest.DTO;

namespace WooliesXTest.Tests
{
    public class MockDataProvider
    {

        public static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product {Name = "A Test Product", Price = 45, Quantity = 1 },
                new Product {Name = "C Test Product", Price = 51, Quantity = 2 },
                new Product {Name = "B Test Product", Price = 43, Quantity = 1 }
            };
        }

        public static string GetProductsString()
        {
            return "[{'name': 'A Test Product','price': 99.99,'quantity': 0},{'name': 'B Test Product','price': 101.99,'quantity': 0},{'name': 'C Test Product','price': 10.99,'quantity': 0}]";
        }

        public static List<ShoppersHistory> GetShoppersHistory()
        {

            var result = new List<ShoppersHistory>();
            result.Add(new ShoppersHistory
            {
                CustomerId = 123,
                Products = new List<Product> {
                new Product {Name = "A Test Product", Price = 45, Quantity = 5 },
                new Product {Name = "B Test Product", Price = 51, Quantity = 2 },
                }
            });
            result.Add(new ShoppersHistory
            {
                CustomerId = 23,
                Products = new List<Product> {
                new Product {Name = "B Test Product", Price = 51, Quantity = 1 },
                }
            });
            result.Add(new ShoppersHistory
            {
                CustomerId = 54,
                Products = new List<Product> {
                new Product {Name = "B Test Product", Price = 51, Quantity = 1 },
                }
            });
            return result;
        }

        public static TrolleyRequest GetTrolleyRequest(int productQuantity, int specialQuantity, bool bSetSpecialNull = false)
        {

            var result = new TrolleyRequest();
            result.Products = new List<TrolleyProduct>
            {
                new TrolleyProduct
                {
                    Name = "A Test Product",
                    Price = 50
                }
            };
            result.Quantities = new List<ProductQuantity>
            {
                new ProductQuantity
                {
                     Name = "A Test Product",
                     Quantity = productQuantity
                }
            };
            if (!bSetSpecialNull)
            {
                result.Specials = new List<Special>
                {
                    new Special
                    {
                         Quantities =  new List<ProductQuantity>
                         {
                             new ProductQuantity
                             {
                                Name = "A Test Product",
                                Quantity = specialQuantity
                             }
                         },
                        Total = 2
                    }
                };
            }
            return result;
        }
    }
}
