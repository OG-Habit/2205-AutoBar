using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<Product> GetProduct(int productID);
    }
}
