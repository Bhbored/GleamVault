using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IShopDataStore
    {
        Task<List<Category>> GetCategories();
        Task<List<Product>> GetItems(Guid CategoryID);
        Task<List<Customer>> GetCustomers();
    }
}
