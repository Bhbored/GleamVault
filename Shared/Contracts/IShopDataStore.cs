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
        Task<Category> SaveCategory(Category category);
        Task<bool> DeleteCategory(Guid Id);
        Task<List<Product>> GetItems();
        Task<Product> SaveProducts(Product product);
        Task<bool> DeleteProduct(Guid ProductID);
        Task<List<Customer>> GetCustomers();
        Task<Customer> SaveCustomers(Customer customer);
        Task<bool> DeleteCustomer(Guid CustomerID);
        Task<List<Transaction>> GetTransactions();
        Task<List<TransactionItem>> GetTransactionItems(Guid TransactionID);
        Task<Transaction> SaveTransactions(Transaction transaction);
        Task<LoginResponse> Login(LoginRequest loginRequest);

    }
}
