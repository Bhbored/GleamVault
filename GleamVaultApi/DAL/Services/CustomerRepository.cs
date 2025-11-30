using GleamVaultApi.DAL.Contracts;
using GleamVaultApi.DB;
using Shared.Models;
using System.Security.Principal;
using Customer = GleamVaultApi.DB.Customer;

namespace GleamVaultApi.DAL.Services
{
    public class CustomerRepository : BaseServiceClass<Customer>, IViewModelResult<Customer, CustomerInfo>
    {
        public async Task<List<CustomerInfo>> GetAllAsViewModel()
        {
            var result = await GetAll();
            return result.Select(MapViewModel).ToList();
        }

        public async Task<CustomerInfo> GetAsViewModel(Guid id)
        {
            var result = await Get(id);
            return MapViewModel(result);
        }

        protected override Customer Map(Customer original, Customer sourceEntity)
        {
            if (original == null || sourceEntity == null)
                return original;

            
            original.FullName = sourceEntity.FullName;
            original.PhoneNumber = sourceEntity.PhoneNumber;
            original.Email = sourceEntity.Email;
            original.DateOfBirth = sourceEntity.DateOfBirth;
            original.Address = sourceEntity.Address;
            original.LoyaltyPoints = sourceEntity.LoyaltyPoints;

            return original;
        }

        public CustomerInfo MapViewModel(DB.Customer entity)
        {
            if (entity == null) return null;

            return new CustomerInfo()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth ?? DateTime.MinValue,
                Address = entity.Address,
                LoyaltyPoints = entity.LoyaltyPoints,
            };
        }

        public async Task<CustomerInfo> SaveAsync(Shared.Models.Customer customerInfo, IIdentity user)
        {
            if (customerInfo == null)
                throw new ArgumentNullException(nameof(customerInfo), "Customer data is required");

            var entity = new DB.Customer()
            {
                Id = customerInfo.Id,
                FullName = customerInfo.FullName,
                PhoneNumber = customerInfo.PhoneNumber,
                Email = customerInfo.Email,
                DateOfBirth = customerInfo.DateOfBirth,
                Address = customerInfo.Address,
                LoyaltyPoints = customerInfo.LoyaltyPoints
            };

            var result = await Update(entity, user);
            return MapViewModel(result);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var deletedEntity = await Delete(id);
                return deletedEntity != null;
            }
            catch (Exception)
            {
                
                return false;
            }
        }

    }
}