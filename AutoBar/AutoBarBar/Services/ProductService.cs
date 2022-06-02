using AutoBarBar.Models;
using AutoBarBar.Services;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using static AutoBarBar.Constants;
using Xamarin.Forms;
using System.Data.Common;

[assembly: Dependency(typeof(ProductService))]
namespace AutoBarBar.Services
{
    public class ProductService : BaseService, IProductService
    {
        readonly List<Product> products = new List<Product>();
        Product u = new Product();

        public async Task<IEnumerable<Product>> GetProducts()
        {
            string cmd = @"
                SELECT * FROM Products
            ";

            // The second argument of the GetItems() is a function. The function is what you want to do with the query result
            // cmd = query string
            // dataRecord = what holds the current row of the query results
            // product = container variable
            GetItems<Product>(cmd, (dataRecord, product) =>
            {
                product.ID = dataRecord.GetInt32(0);
                product.Name = dataRecord.GetString(1);
                product.Description = dataRecord.GetString(2);
                product.UnitPrice = dataRecord.GetDecimal(3);
                product.ImageLink = "default_menu.png";
                products.Add(product);
            });

            return await Task.FromResult(products);
        }

        public async Task<Product> GetProduct(int productID)
        {
            string cmd = @"
                SELECT * FROM Products;

                WHERE ID = {productID};
            ";

            // The second argument of the GetItems() is a function. The function is what you want to do with the query result
            // cmd = query string
            // dataRecord = what holds the current row of the query results
            // product = container variable
            GetItem(cmd, ref u, (dataRecord, user) =>
            {
                u.ID = dataRecord.GetInt32(0);
                u.Name = dataRecord.GetString(1);
                u.Description = dataRecord.GetString(2);
                u.UnitPrice = dataRecord.GetDecimal(3);
                u.ImageLink = dataRecord.GetValue(4).ToString();
                u.ImageLink = "default_menu.png";
            });

            return await Task.FromResult(u);
        }
    }
}
