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

        public async Task<IEnumerable<Product>> GetProducts()
        {
            string cmd = "SELECT * FROM Products";

            // The second argument of the GetItems() is a function. The function is what you want to do with the query result
            // cmd = query string
            // dataRecord = what holds the current row of the query results
            // product = container variable
            GetItems<Product>(cmd, ((dataRecord, product) =>
            {
                product.Id = dataRecord.GetInt32(0);
                product.Name = dataRecord.GetString(1);
                product.Description = dataRecord.GetString(2);
                product.UnitPrice = Convert.ToDouble(dataRecord.GetValue(3));
                product.ImageLink = "default_pic.png";
                products.Add(product);
            }));

            return await Task.FromResult(products);
        }
    }
}
