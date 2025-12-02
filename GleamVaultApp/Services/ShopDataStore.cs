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

        public async Task<bool> DeleteCategory(Guid Id)
        {
            var result = await client.Delete<bool>(Constants.API_DELETE_CATEGORYS, Id);
            return result.IsSuccess;
        }

        public async Task<bool> DeleteCustomer(Guid CustomerID)
        {
            var result=await client.Delete<bool>(Constants.API_DELETE_CUSTOMERS, CustomerID);
            return result.IsSuccess;
        }

        public async Task<bool> DeleteProduct(Guid ProductID)
        {
           var result=await client.Delete<bool>(Constants.API_DELETE_ITEMS, ProductID);
            return result.IsSuccess;
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

        public async Task<List<TransactionItem>> GetTransactionItems(Guid TransactionID)
        {
            var result = await client.Get<List<TransactionItem>>($"{Constants.API_GET_TRANSACTIONITEM}?TransactionID={TransactionID}");
            return result;
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            var result = await client.Get<List<Transaction>>(Constants.API_GET_TRANSACTION);
            return result;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var result = await client.Post<LoginRequest, LoginResponse>(Constants.API_AUTHENTICATION, loginRequest);
            return result.Result;
        }



        public async Task<Category> SaveCategory(Category category)
        {
            var result = await client.Post<Category,Category>(Constants.API_POST_CATEGORYS, category);
            return result.Result;
        }

        public async Task<Customer> SaveCustomers(Customer customer)
        {
            var result = await client.Post<Customer, Customer>(Constants.API_POST_CUSTOMERS, customer);
            return result.Result;
        }

        public async Task<Product> SaveProducts(Product product)
        {
            var result = await client.Post<Product, Product>(Constants.API_POST_ITEMS, product);
            return result.Result;
        }

        public async Task<Transaction> SaveTransactions(Transaction transaction)
        {
            var result = await client.Post<Transaction, Transaction>(Constants.API_POST_TRANSACTION, transaction);
            return result.Result;
        }
    }
}
