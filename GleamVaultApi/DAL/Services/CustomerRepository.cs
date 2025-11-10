using GleamVaultApi.DAL.Contracts;
using GleamVaultApi.DB;
using Shared.Models;
using Customer = GleamVaultApi.DB.Customer;

namespace GleamVaultApi.DAL.Services
{
    public class CustomerRepository : BaseServiceClass<GleamVaultApi.DB.Customer>, IViewModelResult<GleamVaultApi.DB.Customer, CustomerInfo>
    {
        public async Task<List<CustomerInfo>> GetAllAsViewModel()
        {
            var result = await GetAll();
            return result.Select(MapViewModel).ToList();
        }

        public Task<CustomerInfo> GetAsViewModel(Guid id)
        {
            throw new NotImplementedException();
        }
        protected override Customer Map(Customer entity, Customer sourceEnity)
        {
            return null;
        }

        public CustomerInfo MapViewModel(DB.Customer entity)
        {
            return new CustomerInfo()
            {
                Id = entity.Id,
                FullName = entity.FullName,
                PhoneNumber=entity.PhoneNumber,
                Email=entity.Email,
                DateOfBirth=entity.DateOfBirth ?? DateTime.MinValue,
                Address=entity.Address,
                LoyaltyPoints=entity.LoyaltyPoints,
            };
        }
    }
}
