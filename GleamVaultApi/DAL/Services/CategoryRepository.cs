using GleamVaultApi.DAL.Contracts;
using GleamVaultApi.DB;
using Shared.Models;
using Category = GleamVaultApi.DB.Category;

namespace GleamVaultApi.DAL.Services
{
    public class CategoryRepository : BaseServiceClass<Category>, IViewModelResult<Category, CategoryInfo>
    {
        public async Task<List<CategoryInfo>> GetAllAsViewModel()
        {
            var result = await GetAll();
            return result.Select(MapViewModel).ToList();
        }

        public Task<CategoryInfo> GetAsViewModel(Guid id)
        {
            throw new NotImplementedException();
        }
        protected override Category Map(Category entity, Category sourceEnity)
        {
            return null;
        }
        public CategoryInfo MapViewModel(Category entity)
        {
            if (entity == null) return null;

            return new CategoryInfo()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }
    }
}
