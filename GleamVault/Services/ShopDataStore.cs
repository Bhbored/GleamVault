using GleamVault.Services.Interfaces;
using Shared.Contracts;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Services
{
    public class ShopDataStore : IShopDataStore
    {
        IAdvanceHttpService client;
        public ShopDataStore(IAdvanceHttpService _client)
        {
            client = _client;

        }
        public async Task<List<Category>> GetCategories()
        {
            var result = await client.Get<List<Category>>(Constants.API_GET_CATEGORYS);
            return result;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            var result = await client.Get<List<Customer>>(Constants.API_GET_CUSTOMERS);
            return result;
        }

        public async Task<List<Product>> GetItems(Guid CategoryID)
        {
            var url = $"{Constants.API_GET_ITEMS}?categoryId={CategoryID}";
            var result = await client.Get<List<Product>>(url);
            return result;
        }
    }
}
